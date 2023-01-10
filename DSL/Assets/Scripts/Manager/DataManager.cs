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

public class DataManager : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    public TextAsset stations;
    public TextAsset questions;
    public TextAsset answers;
    public TextAsset hints;

    [Header("Options")]
    public string saveFile = "/groupdata.json";

    public event Action OnGroupDataLoaded;
    public event Action<Group> OnNewGroupCreated;
    public event Action OnStationsLoaded;

    private List<Station> _stations = new();
    private List<Question> _questions = new();
    private List<Answer> _answers = new();
    private List<Hint> _hints = new();
    private List<Group> _groups = new();

    // bool to check if we need to rewrite the group file
    private bool _isGroupFileDirty;
    private GroupData groupData;
    private Group currentGroup;

    private static DataManager s_instance;
    #endregion

    #region Properties
    public static DataManager Instance { get { return s_instance; } }

    public List<Station> Stations { get => _stations; private set => _stations = value; }
    public List<Question> Questions { get => _questions; private set => _questions = value; }
    public List<Answer> Answers { get => _answers; private set => _answers = value; }
    public List<Hint> Hints { get => _hints; private set => _hints = value; }
    public List<Group> Groups { get => _groups; private set => _groups = value; }
    public Group CurrentGroup { get => currentGroup; private set => currentGroup = value; }
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
        CreateTestData();
        ReadGroupFile();
        ReadCSVFile();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            AddNewGroup("groupName" + Random.Range(0, 10), Random.Range(0, 100), Random.Range(0, 2));
        }
    }

    private void OnApplicationPause()
    {
        if (_isGroupFileDirty)
        {
            WriteFile();
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
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

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

    private void ReadCSVFile()
    {
        var config = new CsvConfiguration(CultureInfo.GetCultureInfoByIetfLanguageTag("de-DE"))
        {
            Delimiter = ";",
        };

        using (var reader = new StringReader(stations.text))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<StationMap>();
            var records = csv.GetRecords<Station>();

            foreach (var item in records)
            {
                Debug.Log(item.name);
                Stations.Add(item);
            }
        }

        using (var reader = new StringReader(questions.text))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<QuestionMap>();
            var records = csv.GetRecords<Question>();

            foreach (var item in records)
            {
                Debug.Log(item.text);
                Questions.Add(item);
            }
        }

        using (var reader = new StringReader(answers.text))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<AnswerMap>();
            var records = csv.GetRecords<Answer>();

            foreach (var item in records)
            {
                Answers.Add(item);
            }
        }

        using (var reader = new StringReader(hints.text))
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

    private void WriteFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(groupData);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
        Debug.Log("Saved File as: " + saveFile);
    }

    #endregion

    #region Get Data Functions#
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

}
