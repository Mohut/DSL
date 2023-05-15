using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TipScreen tipScreen;
    [SerializeField] private int answerDelay;
    [SerializeField] private Button yesButton;

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
    [SerializeField] private Image buttonImage1;
    [SerializeField] private Button button2;
    [SerializeField] private Image buttonImage2;
    [SerializeField] private Button button3;
    [SerializeField] private Image buttonImage3;
    [SerializeField] private Button button4;
    [SerializeField] private Image buttonImage4;
    [SerializeField] private Button homeButton;
    [SerializeField] private Sprite purple;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite red;

    private float time;
    private int currentQuestionCount = 1;
    void Start()
    {
        GameManager.Instance.ResetUsedTips();
        
        groupName.text = GameManager.Instance.CurrentGroup.name;
        subject.text = GameManager.Instance.CurrentStation.name;
        question.text = GameManager.Instance.CurrentQuestion.text;
        task.text = "Aufgabe " + currentQuestionCount + " / " + GameManager.Instance.CurrentStation.questionId.Count;
        questionPoints.text = GameManager.Instance.CurrentQuestion.points.ToString();
        
        yesButton.onClick.AddListener(SceneManager.LoadMainMenu);

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
        
        if (time < 60)
        {
            timeText.text = "Weniger als 1 Min.";
        }
        else
        {
            timeText.text = (int)time / 60 + " Min.";
        }
            
        slider.value = time / GameManager.Instance.CurrentStation.time;
        
        if(time <= 0)
            SceneManager.LoadEndScreen();
    }

    private void SetAnswerQuestionText()
    {
        question.text = GameManager.Instance.CurrentQuestion.text;
        
        if(GameManager.Instance.CurrentAnswers.Count >= 1)
            answer1.text = GameManager.Instance.CurrentAnswers[0].text;
        
        if(GameManager.Instance.CurrentAnswers.Count >= 2)
            answer2.text = GameManager.Instance.CurrentAnswers[1].text;
        
        if (GameManager.Instance.CurrentAnswers.Count >= 3)
        {
            button3.interactable = true;
            buttonImage3.enabled = true;
            answer3.enabled = true;
            answer3.text = GameManager.Instance.CurrentAnswers[2].text;
        }
        else
        {
            button3.interactable = false;
            buttonImage3.enabled = false;
            answer3.enabled = false;
        }

        if (GameManager.Instance.CurrentAnswers.Count >= 4)
        {
            button4.interactable = true;
            buttonImage4.enabled = true;
            answer4.enabled = true;
            answer4.text = GameManager.Instance.CurrentAnswers[3].text;
        }
        else
        {
            button4.interactable = false;
            buttonImage4.enabled = false;
            answer4.enabled = false;
        }
            
    }

    private bool CheckAnswer(int index)
    {
        if (GameManager.Instance.CurrentAnswers[index].isCorrect)
        {
            GameManager.Instance.CurrentGroup.points += GameManager.Instance.CurrentQuestion.points;
            currentScoreText.text = "Score: " + GameManager.Instance.CurrentGroup.points;
            return true;
        }

        return false;
    }

    private void UpdateCurrentQuestionData()
    {
        currentQuestionCount++;
        task.text = "Aufgabe " + currentQuestionCount + " / " + GameManager.Instance.CurrentStation.questionId.Count;
        questionPoints.text = GameManager.Instance.CurrentQuestion.points.ToString();
    }

    private void LoadNewQuestion()
    {
        if (GameManager.Instance.SetNextQuestion() == false)
        {
            DataManager.Instance.SendResults();
            SceneManager.LoadEndScreen();
        }
    }

    private void ShowIfCorrect(Image button, bool correct)
    {
        if(correct)
            button.sprite = green;
        
        if(!correct)
            button.sprite = red;
    }

    private void MakeButtonGrey(Image button)
    {
        button.sprite = purple;
    }

    IEnumerator ShowNextQuestion(Image button, int answerIndex)
    {
        ShowIfCorrect(button, GameManager.Instance.CurrentAnswers[answerIndex].isCorrect);
        
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;

        yield return new WaitForSeconds(answerDelay);
        
        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
        button4.interactable = true;
        
        LoadNewQuestion();
        MakeButtonGrey(button);
        tipScreen.ResetTip();
        SetAnswerQuestionText();
        UpdateCurrentQuestionData();
    }

    private void SetButtonMethods()
    {
        button1.onClick.AddListener(() =>
        {
            GameManager.Instance.AdChosenAnswer(currentQuestionCount, 0, CheckAnswer(0));
            StartCoroutine(ShowNextQuestion(buttonImage1,0));
            tipScreen.ShowTipButton(GameManager.Instance.CurrentHint != null);
        });
        
        button2.onClick.AddListener( () => {
            GameManager.Instance.AdChosenAnswer(currentQuestionCount, 1, CheckAnswer(1));
            StartCoroutine(ShowNextQuestion(buttonImage2, 1));
            tipScreen.ShowTipButton(GameManager.Instance.CurrentHint != null);
        });
        
        button3.onClick.AddListener(() =>
        {
            GameManager.Instance.AdChosenAnswer(currentQuestionCount, 2, CheckAnswer(2));
            StartCoroutine(ShowNextQuestion(buttonImage3, 2));
            tipScreen.ShowTipButton(GameManager.Instance.CurrentHint != null);
        });
        
        button4.onClick.AddListener(() =>
        {
            GameManager.Instance.AdChosenAnswer(currentQuestionCount, 3, CheckAnswer(3));
            StartCoroutine(ShowNextQuestion(buttonImage4, 3));
            tipScreen.ShowTipButton(GameManager.Instance.CurrentHint != null);
        });
    }
}
