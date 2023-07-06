using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Attributes
    private static GameManager s_instance;
    private Station _currentStation;
    private Question _currentQuestion;
    private List<Answer> _currentAnswers = new();
    private Hint _currentHint;
    private Group _currentGroup;
    private List<Group> _playsessionsGroups = new();

    private int _usedTips;
    private int _allUsedTips;
    private int _questionIteration;
    private bool _paidForHint;
    private bool _skipLogin;
    private List<ChosenAnswer> chosenAnswers = new List<ChosenAnswer>();
    #endregion

    #region Properties
    public static GameManager Instance { get { return s_instance; } }
    public Station CurrentStation { get => _currentStation; private set => _currentStation = value; }
    public Question CurrentQuestion { get => _currentQuestion; private set => _currentQuestion = value; }
    public List<Answer> CurrentAnswers { get => _currentAnswers; private set => _currentAnswers = value; }
    public Hint CurrentHint { get => _currentHint; private set => _currentHint = value; }
    public int QuestionIteration { get => _questionIteration; set => _questionIteration = value; }
    public Group CurrentGroup { get => _currentGroup; set => _currentGroup = value; }
    public bool PaidForHint { get => _paidForHint; set => _paidForHint = value; }
    public int UsedTips { get => _usedTips; set => _usedTips = value; }
    public int AllUsedTips { get => _allUsedTips; set => _allUsedTips = value; }
    public List<ChosenAnswer> ChosenAnswers { get => chosenAnswers; set => chosenAnswers = value; }
    public List<Group> PlaysessionsGroups { get => _playsessionsGroups; set => _playsessionsGroups = value; }
    public bool SkipLogin { get => _skipLogin; set => _skipLogin = value; }

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
        DataManager.Instance.OnNewGroupCreated += SetCurrentGroup;
    }

    private void OnDestroy()
    {
        DataManager.Instance.OnNewGroupCreated -= SetCurrentGroup;
    }
    #endregion

    public void SetCurrentGroup(Group group)
    {
        CurrentGroup = group;
        PlaysessionsGroups.Add(CurrentGroup);
    }

    public void SetCurrentStation(Station station)
    {
        CurrentStation = station;
        // Set current question to first question
        CurrentQuestion = DataManager.Instance.GetQuestionById(station.questionId[0]);
        CurrentAnswers = DataManager.Instance.GetAnswersById(CurrentQuestion.answerId.ToList());

        if(!string.IsNullOrEmpty(CurrentQuestion.hintId))
            CurrentHint = DataManager.Instance.GetHintById(int.Parse(CurrentQuestion.hintId));

        CurrentGroup.stationId = CurrentStation.id;

        QuestionIteration = 0;
        PaidForHint = false;
    }

    // Selects the next question and returns if there is a new one
    public bool SetNextQuestion()
    {
        QuestionIteration += 1;

        if (CurrentStation.questionId.Count <= QuestionIteration)
            return false;
        
        Question nextQuestion = DataManager.Instance.GetQuestionById(CurrentStation.questionId[QuestionIteration]);

        if (nextQuestion != null)
        {
            CurrentQuestion = DataManager.Instance.GetQuestionById(CurrentStation.questionId[QuestionIteration]);
            CurrentAnswers = DataManager.Instance.GetAnswersById(CurrentQuestion.answerId.ToList());

            if (!string.IsNullOrEmpty(CurrentQuestion.hintId))
            {
                CurrentHint = DataManager.Instance.GetHintById(int.Parse(CurrentQuestion.hintId));
            }
            else
            {
                CurrentHint = null;
            }    

            PaidForHint = false;
        }

        return true;
    }

    public void SetResult(Answer answer, bool lastQuestion = false)
    {
        Result result = new Result();
        result.station = CurrentStation.name;
        result.question = CurrentQuestion.text;
        result.usedHint = PaidForHint;
        result.groupName = CurrentGroup.name;

        if(lastQuestion)
            result.points = CurrentGroup.points.ToString();

        if (answer.isCorrect)
        {
            result.isCorrect = true;
        }
        else
        {
            result.isCorrect = false;
        }

        DataManager.Instance.Results.Add(result);
    }

    public void PayForHint()
    {
        if(!PaidForHint)
        {
            CurrentGroup.points -= CurrentHint.price;
            PaidForHint = true;
        }
    }

    public void ResetUsedTips()
    {
        _usedTips = 0;
    }

    public void ClearChosenAnswers()
    {
        chosenAnswers.Clear();
    }

    public void AdChosenAnswer(int taskNumber, bool right)
    {
        if(right)
            Instance.CurrentGroup.points += Instance.CurrentQuestion.points;
        chosenAnswers.Add(new ChosenAnswer(taskNumber, _currentQuestion, right,  _usedTips));
    }
}
