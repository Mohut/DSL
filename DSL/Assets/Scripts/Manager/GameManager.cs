using System.Collections.Generic;
using System.Linq;
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

    private int _currentScore;
    private int _usedTips;
    private int _questionIteration;
    private bool _payedForHint;
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
    public bool PayedForHint { get => _payedForHint; set => _payedForHint = value; }
    public int CurrentScore { get => _currentScore; set => _currentScore = value; }
    public int UsedTips { get => _usedTips; set => _usedTips = value; }
    public List<ChosenAnswer> ChosenAnswers { get => chosenAnswers; set => chosenAnswers = value; }

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
    }

    public void SetCurrentStation(Station station)
    {
        CurrentStation = station;
        // Set current question to first question
        CurrentQuestion = DataManager.Instance.GetQuestionById(station.questionId[0]);
        CurrentAnswers = DataManager.Instance.GetAnswersById(CurrentQuestion.answerId.ToList());
        CurrentHint = DataManager.Instance.GetHintById(CurrentQuestion.hintId);
        CurrentGroup.stationId = CurrentStation.id;

        QuestionIteration = 0;
        PayedForHint = false;
    }

    // Selects the next question and returns if there is a new one
    public bool SetNextQuestion()
    {
        QuestionIteration += 1;

        if (CurrentStation.questionId.Length <= QuestionIteration)
            return false;
        
        Question nextQuestion = DataManager.Instance.GetQuestionById(CurrentStation.questionId[QuestionIteration]);

        if (nextQuestion != null)
        {
            CurrentQuestion = DataManager.Instance.GetQuestionById(CurrentStation.questionId[QuestionIteration]);
            CurrentAnswers = DataManager.Instance.GetAnswersById(CurrentQuestion.answerId.ToList());
            CurrentHint = DataManager.Instance.GetHintById(CurrentQuestion.hintId);
            PayedForHint = false;
        }

        return true;
    }

    public void CheckAnswer(Answer answer)
    {
        if (answer.isCorrect)
        {
            CurrentGroup.points += CurrentQuestion.points;
            SetNextQuestion();
        }
        else
        {
            SetNextQuestion();
        }
    }

    public void PayForHint()
    {
        if(!PayedForHint)
        {
            CurrentGroup.points -= CurrentHint.price;
            PayedForHint = true;
        }
    }

    public void ResetScore()
    {
        _currentScore = 0;
    }

    public void ResetUsedTips()
    {
        _usedTips = 0;
    }

    public void ClearChosenAnswers()
    {
        chosenAnswers.Clear();
    }

    public void AdChosenAnswer(int taskNumber, int answerIndex, bool right)
    {
        chosenAnswers.Add(new ChosenAnswer(taskNumber, _currentQuestion, _currentAnswers[answerIndex], right,  _usedTips));
    }
}
