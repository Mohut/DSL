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
    public string saveFile = "/groupdata.json";
    public List<string> sheetNames = new List<string>();

    public event Action OnGroupDataLoaded;
    public event Action<Group> OnNewGroupCreated;
    public event Action OnStationsLoaded;

    private const string API_KEY = "AIzaSyD0updEet8t1bsPScz8tmOHLfk2prxCOq0";
    private const string SPREADSHEET_ID = "1YZyTv9T4fjRKxjxW0VuyW1UZIlyHdW8_2MfIIdpsLT8";
    private const string FOLDER_ID = "1k-vL2YuYqPf6XbzVzOf7-6fASuvbGVPG";
    private const string SERVICE_ACC = "serviceaccount@spring-radar-369315.iam.gserviceaccount.com";
    private List<Station> _stations = new();
    private List<Question> _questions = new();
    private List<Answer> _answers = new();
    private List<Hint> _hints = new();
    private List<Group> _groups = new();
    private List<Result> _results = new();

    // bool to check if we need to rewrite the group file
    private bool _isGroupFileDirty;
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
        Debug.Log(saveFile);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        foreach (string sheet in sheetNames)
        {
            DownloadSheet(sheet);
        }
        
        CreateTestData();
        ReadGroupFile();
        ReadCSVFile();
        //UploadSheet();
    }

    private void OnApplicationPause()
    {
        if (_isGroupFileDirty)
        {
            WriteFile();
        }
    }
    #endregion

    #region CSV Download Functions
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
        Debug.Log("Saved as: " + Application.persistentDataPath + "/" + name + ".CSV");
        using (var writer = new StreamWriter(Application.persistentDataPath + "/" + name + ".CSV"))
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
            Debug.Log("Key JSON: " + serviceKeyPath.text);
            var credentials = GoogleCredential.FromJson(serviceKeyPath.text).CreateScoped(DriveService.ScopeConstants.Drive);
            Debug.Log("Google credential: " + credentials);
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

    private void ReadCSVFile()
    {
        var config = new CsvConfiguration(CultureInfo.GetCultureInfoByIetfLanguageTag("de-DE"))
        {
            Delimiter = ";",
            MissingFieldFound = null,
    };

        using (var reader = new StreamReader(Application.persistentDataPath + "/Station.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<StationMap>();
            var records = csv.GetRecords<Station>();

            foreach (var item in records)
            {
                Stations.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + "/Question.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<QuestionMap>();
            var records = csv.GetRecords<Question>();

            foreach (var item in records)
            {
                Questions.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + "/Answer.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<AnswerMap>();
            var records = csv.GetRecords<Answer>();

            foreach (var item in records)
            {
                Answers.Add(item);
            }
        }

        using (var reader = new StreamReader(Application.persistentDataPath + "/Hint.csv"))
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
        _isGroupFileDirty = true;
        OnNewGroupCreated?.Invoke(group);
        return group;
    }

    private void ReadGroupFile()
    {
        // Does the file exist?
        if (System.IO.File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = System.IO.File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            groupData = JsonUtility.FromJson<GroupData>(fileContents);
            Groups = new List<Group>(groupData.groupData);
            Debug.Log(Groups.Count + " groups have been read.");
            OnGroupDataLoaded?.Invoke();
        }
        else
        {
            Debug.Log("Group file doesnt exist.");
        }
    }

    private void WriteFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(groupData);

        // Write JSON to file.
        System.IO.File.WriteAllText(saveFile, jsonString);
        Debug.Log("Saved File as: " + saveFile);
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

    #region Testing Functions
    private void CreateTestData()
    {
        /*
        Station station1 = new Station();
        station1.id = 0;
        station1.name = "Privatssphäre Station";
        station1.time = 200;
        station1.questionId = new int[2]{ 0, 1 };

        Station station2 = new Station();
        station2.id = 1;
        station2.name = "Datenerhebungsstation";
        station2.time = 150;
        station2.questionId = new int[2] { 2, 3 };

        Question question1 = new Question();
        question1.id = 0;
        question1.text = "question 1 text";
        question1.type = "default";
        question1.points = 10;
        question1.hintId = 0;
        question1.answerId = new int[2] {0, 1};

        Question question2 = new Question();
        question2.id = 1;
        question2.text = "question 2 text";
        question2.type = "default";
        question2.points = 10;
        question2.hintId = 1;
        question2.answerId = new int[2] { 2, 3 };

        Question question3 = new Question();
        question3.id = 2;
        question3.text = "question 3 text";
        question3.type = "default";
        question3.points = 10;
        question3.hintId = 2;
        question3.answerId = new int[2] { 4, 5 };

        Question question4 = new Question();
        question4.id = 3;
        question4.text = "question 4 text";
        question4.type = "default";
        question4.points = 10;
        question4.hintId = 3;
        question4.answerId = new int[2] { 6, 7 };

        Answer answer1 = new Answer();
        answer1.id = 0;
        answer1.text = "answer 1 text";
        answer1.isCorrect = true;

        Answer answer2 = new Answer();
        answer2.id = 1;
        answer2.text = "answer 2 text";
        answer2.isCorrect = false;

        Answer answer3 = new Answer();
        answer3.id = 2;
        answer3.text = "answer 3 text";
        answer3.isCorrect = true;

        Answer answer4 = new Answer();
        answer4.id = 3;
        answer4.text = "answer 4 text";
        answer4.isCorrect = false;

        Answer answer5 = new Answer();
        answer5.id = 4;
        answer5.text = "answer 5 text";
        answer5.isCorrect = true;

        Answer answer6 = new Answer();
        answer6.id = 5;
        answer6.text = "answer 6 text";
        answer6.isCorrect = false;

        Answer answer7 = new Answer();
        answer7.id = 6;
        answer7.text = "answer 7 text";
        answer7.isCorrect = true;

        Answer answer8 = new Answer();
        answer8.id = 7;
        answer8.text = "answer 8 text";
        answer8.isCorrect = false;

        Hint hint1 = new Hint();
        hint1.id = 0;
        hint1.text = "hint1 text";

        Hint hint2 = new Hint();
        hint2.id = 1;
        hint2.text = "hint2 text";

        Hint hint3 = new Hint();
        hint3.id = 2;
        hint3.text = "hint3 text";

        Hint hint4 = new Hint();
        hint4.id = 3;
        hint4.text = "hint4 text";

        Stations.Add(station1);
        Stations.Add(station2);

        Questions.Add(question1);
        Questions.Add(question2);
        Questions.Add(question3);
        Questions.Add(question4);

        Answers.Add(answer1);
        Answers.Add(answer2);
        Answers.Add(answer3);
        Answers.Add(answer4);
        Answers.Add(answer5);
        Answers.Add(answer6);
        Answers.Add(answer7);
        Answers.Add(answer8);

        Hints.Add(hint1);
        Hints.Add(hint2);
        Hints.Add(hint3);
        Hints.Add(hint4);

        OnStationsLoaded?.Invoke();
        */
    }

    #endregion

    #region Result
    public void ClearResults()
    {
        Results.Clear();
    }
    #endregion

}
