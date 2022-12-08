using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TipScreen tipScreen;

    [Header("Textfields")]
    [SerializeField] private TextMeshProUGUI groupName;
    [SerializeField] private TextMeshProUGUI task;
    [SerializeField] private TextMeshProUGUI subject;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private TextMeshProUGUI questionPoints;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI answer1;
    [SerializeField] private TextMeshProUGUI answer2;
    [SerializeField] private TextMeshProUGUI answer3;
    [SerializeField] private TextMeshProUGUI answer4;

    [Header("Buttons")]
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    [SerializeField] private Button homeButton;

    private float time;
    private int currentQuestionCount = 1 ;
    void Start()
    {
        GameManager.Instance.ResetScore();
        GameManager.Instance.ResetUsedTips();
        
        groupName.text = GameManager.Instance.CurrentGroup.name;
        subject.text = GameManager.Instance.CurrentStation.name;
        question.text = GameManager.Instance.CurrentQuestion.text;
        task.text = "Aufgabe " + currentQuestionCount + " / " + GameManager.Instance.CurrentStation.questionId.Length;
        questionPoints.text = GameManager.Instance.CurrentQuestion.points.ToString();
        
        homeButton.onClick.AddListener(SceneManager.LoadMainMenu);

        time = GameManager.Instance.CurrentStation.time;
        
        SetAnswerQuestionText();
        SetButtonMethods();
    }

    private void OnDisable()
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        time -= Time.deltaTime;
        timeText.text = (int)time + " Min.";
    }

    private void SetAnswerQuestionText()
    {
        question.text = GameManager.Instance.CurrentQuestion.text;
        
        if(GameManager.Instance.CurrentAnswers.Count >= 1)
            answer1.text = GameManager.Instance.CurrentAnswers[0].text;
        if(GameManager.Instance.CurrentAnswers.Count >= 2)
            answer2.text = GameManager.Instance.CurrentAnswers[1].text;
        if(GameManager.Instance.CurrentAnswers.Count >= 3)
            answer3.text = GameManager.Instance.CurrentAnswers[2].text;
        if(GameManager.Instance.CurrentAnswers.Count >= 4)
            answer4.text = GameManager.Instance.CurrentAnswers[3].text;
    }

    private void CheckAnswer(int index)
    {
        if (GameManager.Instance.CurrentAnswers[index].isCorrect)
        {
            GameManager.Instance.CurrentScore += GameManager.Instance.CurrentQuestion.points;
            currentScoreText.text = "Score: " + GameManager.Instance.CurrentScore;
        }
    }

    private void UpdateCurrentQuestionData()
    {
        currentQuestionCount++;
        task.text = "Aufgabe " + currentQuestionCount + " / " + GameManager.Instance.CurrentStation.questionId.Length;
        questionPoints.text = GameManager.Instance.CurrentQuestion.points.ToString();
    }

    private void LoadNewQuestion()
    {
        if (GameManager.Instance.SetNextQuestion() == false)
        {
            SceneManager.LoadEndScreen();
        }
    }

    private void SetButtonMethods()
    {
        button1.onClick.AddListener(() =>
        {
            CheckAnswer(0);
            LoadNewQuestion();
            SetAnswerQuestionText();
            UpdateCurrentQuestionData();
            tipScreen.ResetTip();
        });
        
        button2.onClick.AddListener( () => {
            CheckAnswer(1);
            LoadNewQuestion();
            SetAnswerQuestionText();
            UpdateCurrentQuestionData();
            tipScreen.ResetTip();
        });
        
        button3.onClick.AddListener(() =>
        {
            CheckAnswer(2);
            LoadNewQuestion();
            SetAnswerQuestionText();
            UpdateCurrentQuestionData();
            tipScreen.ResetTip();
        });
        
        button4.onClick.AddListener(() =>
        {
            CheckAnswer(3);
            LoadNewQuestion();
            SetAnswerQuestionText();
            UpdateCurrentQuestionData();
            tipScreen.ResetTip();
        });
    }
}