using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Upload;
using System.Threading.Tasks;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;

public class DataManager : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    public TextAsset stations;
    public TextAsset questions;
    public TextAsset answers;
    public TextAsset hints;
    public TextAsset serviceKeyPath;

    [Header("Options")]
    private string saveFile = Path.DirectorySeparatorChar + "groupdata.json";
    public List<string> sheetNames = new List<string>();

    public event Action OnGroupDataLoaded;
    public event Action<Group> OnNewGroupCreated;
    public event Action OnStationsLoaded;

    private const string API_KEY = "AIzaSyD0updEet8t1bsPScz8tmOHLfk2prxCOq0";
    private const string SPREADSHEET_ID = "1YZyTv9T4fjRKxjxW0VuyW1UZIlyHdW8_2MfIIdpsLT8";
    private const string FOLDER_ID = "1k-vL2YuYqPf6XbzVzOf7-6fASuvbGVPG";
    private const string FOLDER_ID_PROCESSED = "1wqmn1YGOhHQok7kTXv2rFT5T4q6Y_3a_";
    private List<Station> _stations = new();
    private List<Question> _questions = new();
    private List<Answer> _answers = new();
    private List<Hint> _hints = new();
    private List<Group> _groups = new();
    private List<Result> _results = new();

    // bool to check if we need to rewrite the group file
    private GroupData groupData;
    private string lastGroupName;

    private static DataManager s_instance;
    #endregion

    #region Properties
    public static DataManager Instance { get { return s_instance; } }

    public List<Station> Stations { get => _stations; private set => _stations = value; }
    public List<Question> Questions { get => _questions; private set => _questions = value; }
    public List<Answer> Answers { get => _answers; private set => _answers = value; }
    public List<Hint> Hints { get => _hints; private set => _hints = value; }
    public List<Group> Groups { get => _groups; private set => _groups = value; }
    public List<Result> Results { get => _results; set => _results = value; }
    public string LastGroupName { get => lastGroupName; set => lastGroupName = value; }
    #endregion

    #region Monobehavior Functions
    private void Awake()
    {
        // Singleton Pattern
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
        }

        groupData = new GroupData();
        saveFile = Application.persistentDataPath + saveFile;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        foreach (string sheet in sheetNames)
        {
            DownloadSheet(sheet);
        }
        
        ReadCSVFile();
    }

    private void OnApplicationPause(bool pause)
    {
        DeleteFilesInFolder();    
    }

    #endregion

    #region CSV Drive Functions
    private void DownloadSheet(string name)
    {
        string apiKey = API_KEY;
        var initializer = new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
        };

        var service = new SheetsService(initializer);

        string spreadsheetId = SPREADSHEET_ID;
        string range = name;

        var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
        var response = request.Execute();
        using (var writer = new StreamWriter(Application.persistentDataPath + Path.DirectorySeparatorChar + name + ".CSV"))
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
            };

            var csv = new CsvWriter(writer, config);

            foreach (var row in response.Values)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    var cellValue = row[i]?.ToString();
                    csv.WriteField(cellValue);
                }
                csv.NextRecord();
            }
        }
    }

    public void UploadSheet()
    {
        try
        {
            var credentials = GoogleCredential.FromJson(serviceKeyPath.text).CreateScoped(DriveService.ScopeConstants.Drive);
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials
            });

            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream))
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.Context.RegisterClassMap<ResultMap>();
                    csvWriter.WriteRecords(Results);
                    streamWriter.Flush(); // Flush the StreamWriter to ensure data is written to the stream.

                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = "result.CSV",
                        Parents = new[] { FOLDER_ID },
                    };

                    var uploadRequest = service.Files.Create(fileMetadata, stream, "text/csv");
                    uploadRequest.Upload();
                } // StreamWriter gets flushed here.

            }

            ClearResults();
        }
        catch (Exception ex) 
        {
            Debug.LogError("Error creating Google credential: " + ex.Message);
        }
    }

    public void DeleteFilesInFolder()
    {
        var credentials = GoogleCredential.FromJson(serviceKeyPath.text).CreateScoped(DriveService.ScopeConstants.Drive);
        var service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials
        });

        // Retrieve all files in the specified folder
        var fileListRequest = service.Files.List();
        fileListRequest.Q = $"'{FOLDER_ID_PROCESSED}' in parents and trashed = false";
        var files = fileListRequest.Execute().Files;

        // Delete each file in the folder
        foreach (var file in files)
        {
            service.Files.Delete(file.Id).Execute();
        }
    }

    private void ReadCSVFile()
    {
        var config = new CsvConfiguration(CultureInfo.GetCultureInfoByIetfLanguageTag("de-DE"))
        {
            Delimiter = ";",
            MissingFieldFound = null,
    };

        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + "Station.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<StationMap>();
            var records = csv.GetRecords<Station>();

            foreach (var item in records)
            {
                Stations.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + "Question.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<QuestionMap>();
            var records = csv.GetRecords<Question>();

            foreach (var item in records)
            {
                Questions.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + "Answer.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<AnswerMap>();
            var records = csv.GetRecords<Answer>();

            foreach (var item in records)
            {
                Answers.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + "Hint.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<HintMap>();
            var records = csv.GetRecords<Hint>();

            foreach (var item in records)
            {
                Hints.Add(item);
            }
        }
    }
    #endregion

    #region Group Functions
    public Group AddNewGroup(string name, int points = 0, int stationId = 0)
    {
        Group group = new Group(name, 0);
        group.name = name;
        group.points = points;
        group.stationId = stationId;
        groupData.groupData.Add(group);
        OnNewGroupCreated?.Invoke(group);
        return group;
    }

    #endregion

    #region Get Data Functions
    public Question GetQuestionById(int id)
    {
        return Questions.Find(x => x.id == id);
    }

    public List<Answer> GetAnswersById(List<int> idList)
    {
        List<Answer> answers = new List<Answer>();

        foreach (Answer answer in Answers)
        {
            if(idList.Contains(answer.id))
            {
                answers.Add(answer);
            }
        }

        return answers;
    }

    public Hint GetHintById(int id)
    {
        return Hints.Find(x => x.id == id);
    }
    #endregion

    #region Result
    public void ClearResults()
    {
        Results.Clear();
    }
    #endregion

}
