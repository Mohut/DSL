using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4.Data;

public class DataManager : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    public TextAsset serviceKeyPath;

    [Header("Options")]
    public List<string> sheetNames = new List<string>();

    public event Action OnGroupDataLoaded;
    public event Action<Group> OnNewGroupCreated;
    public event Action OnStationsLoaded;

    public string debug;

    private const string API_KEY = "AIzaSyD0updEet8t1bsPScz8tmOHLfk2prxCOq0";
    private const string ROOT_FOLDER_ID = "1Uc_NfmXtWTIiU2RC_Gk51JXjaafgRFbA";
    private const string FOLDER_ID = "1k-vL2YuYqPf6XbzVzOf7-6fASuvbGVPG";
    private const string FOLDER_ID_PROCESSED = "1wqmn1YGOhHQok7kTXv2rFT5T4q6Y_3a_";
    private const string FOLDER_ID_SHEETS = "1NT-umDzynzMN0hI_mXAJpQ3c98Z1myhk";

    private Dictionary<string, string> _dataSheets = new Dictionary<string, string>();

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

    // Key: Name der CSV, Value: ID der CSV
    public Dictionary<string, string> DataSheets { get => _dataSheets; set => _dataSheets = value; }

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
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        DataSheets = GetAllFileIdsInFolder();

        foreach (var item in DataSheets)
        {
            foreach (var sheet in sheetNames)
            {
                DownloadSheet(sheet, item.Value, item.Key);
            }
        }
    }

    public void TestReadData()
    {
        foreach (var sheet in DataSheets)
        {
            Debug.Log(sheet.Key);
            if (sheet.Key == "Soziale Arbeit")
            {
                ReadCSVFile(sheet.Key);
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        DeleteFilesInFolder();    
    }

    #endregion

    #region CSV Drive Functions
    private Dictionary<string, string> GetAllFileIdsInFolder()
    {
        Dictionary<string, string> fileIds = new Dictionary<string, string>();

        try
        {
            string apiKey = API_KEY;
            var driveService = new DriveService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
            });

            // Retrieve the list of files within the specified folder
            var driveFilesRequest = driveService.Files.List();
            driveFilesRequest.Q = $"'{ROOT_FOLDER_ID}' in parents";
            var driveFilesResponse = driveFilesRequest.Execute();
            
            // Iterate through each file in the folder
            foreach (var driveFile in driveFilesResponse.Files)
            {
                if (driveFile.MimeType == "application/vnd.google-apps.folder")
                {
                    if(driveFile.Id == FOLDER_ID_SHEETS)
                    {
                        // Retrieve the list of files within the specified folder
                        var driveFilesRequest2 = driveService.Files.List();
                        driveFilesRequest2.Q = $"'{FOLDER_ID_SHEETS}' in parents";
                        var driveFilesResponse2 = driveFilesRequest2.Execute();

                        foreach (var driveFile2 in driveFilesResponse2.Files)
                        {
                            fileIds.Add(driveFile2.Name, driveFile2.Id);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            debug = ex.Message;
        }

        return fileIds;
    }

    private void DownloadSheet(string name, string sheetId, string sheetName)
    {
        try
        {
            string apiKey = API_KEY;
            var initializer = new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
            };

            var service = new SheetsService(initializer);

            string spreadsheetId = sheetId;
            string range = name;

            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();

            Debug.Log(Application.persistentDataPath + Path.DirectorySeparatorChar + sheetName + "_" + name + ".csv");
            using (var writer = new StreamWriter(Application.persistentDataPath + Path.DirectorySeparatorChar + sheetName + "_" + name + ".csv"))
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
        catch (Exception ex) 
        {
            debug = ex.Message;
        }
    }

    public void UploadResult()
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

    public void ReadCSVFile(string sheetName)
    {
        var config = new CsvConfiguration(CultureInfo.GetCultureInfoByIetfLanguageTag("de-DE"))
        {
            Delimiter = ";",
            MissingFieldFound = null,
    };
        Debug.Log(Application.persistentDataPath + Path.DirectorySeparatorChar + sheetName + "_" + "Station.csv");
        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + sheetName + "_" + "Station.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<StationMap>();
            var records = csv.GetRecords<Station>();

            foreach (var item in records)
            {
                Stations.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + sheetName + "_" + "Question.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<QuestionMap>();
            var records = csv.GetRecords<Question>();

            foreach (var item in records)
            {
                Questions.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + sheetName + "_" + "Answer.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<AnswerMap>();
            var records = csv.GetRecords<Answer>();

            foreach (var item in records)
            {
                Answers.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + Path.DirectorySeparatorChar + sheetName + "_" + "Hint.csv"))
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
