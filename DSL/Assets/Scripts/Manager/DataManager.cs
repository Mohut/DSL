using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region Attributes
    private List<Station> _stations = new();
    private List<Question> _questions = new();
    private List<Answer> _answers = new();
    private List<Hint> _hints = new();
    private List<Group> _groups = new();

    // bool to check if we need to rewrite the group file
    private bool _isGroupFileDirty;

    private static DataManager s_instance;
    #endregion

    #region Properties
    public static DataManager Instance { get { return s_instance; } }

    public List<Station> Stations { get => _stations; private set => _stations = value; }
    public List<Question> Questions { get => _questions; private set => _questions = value; }
    public List<Answer> Answers { get => _answers; private set => _answers = value; }
    public List<Hint> Hints { get => _hints; private set => _hints = value; }
    public List<Group> Groups { get => _groups; private set => _groups = value; }
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

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        CreateTestData();
    }

    private void OnApplicationQuit()
    {
        if (_isGroupFileDirty)
        {
            WriteGroupFile(Groups);
        }
    }
    #endregion

    #region Group Functions
    public void AddNewGroup(string name)
    {
        Group group = new();
        group.name = name;
        group.points = 0;
        Groups.Add(group);
        _isGroupFileDirty = true;
    }

    private void WriteGroupFile(List<Group> groups)
    {
        
    }

    #endregion

    #region Testing Functions
    private void CreateTestData()
    {
        Station station1 = new Station();
        station1.id = 0;
        station1.name = "Privatssphäre Station";
        station1.time = 20;
        station1.questionId = new int[2]{ 0, 1 };

        Station station2 = new Station();
        station2.id = 1;
        station2.name = "Datenerhebungsstation";
        station2.time = 15;
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
    }

    #endregion
}
