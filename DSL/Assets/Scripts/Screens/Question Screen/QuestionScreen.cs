using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : MonoBehaviour
{
    [SerializeField] private TipScreen tipScreen;
    [SerializeField] private int answerDelay;
    [SerializeField] private Button yesButton;
    [SerializeField] private GameObject choiceGameObject;
    [SerializeField] private GameObject sequenceGameObject;

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
    List<Button> answerButtons;

    [Header("Sequence")]
    [SerializeField] private Transform sequenceItemParent;
    [SerializeField] private GameObject sequenceItem;
    [SerializeField] private RectTransform sequenceContentTransform;
    private Dictionary<TextMeshProUGUI, int> numberTMPList = new Dictionary<TextMeshProUGUI, int>();
    private List<Image> sequenceImages = new List<Image>();
    private List<Button> sequenceButtons = new List<Button>();
    private List<GameObject> sequenceObjects = new List<GameObject>();
    private int sequenceCounter;

    private float time;
    private int currentQuestionCount = 1;

    void Start()
    {
        answerButtons = new List<Button>() { button1, button2, button3, button4 };
        SetUpUpperPanel();
        GameManager.Instance.ResetUsedTips();
        yesButton.onClick.AddListener(SceneManager.LoadMainMenu);
        time = GameManager.Instance.CurrentStation.time;
        homeButton.onClick.AddListener(SceneManager.LoadMainMenu);

        SetAnswerAndQuestionText();
        SetButtonMethods();
    }

    private void OnDisable()
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
    }

    private void SetUpUpperPanel()
    {
        groupName.text = GameManager.Instance.CurrentGroup.name;
        subject.text = GameManager.Instance.CurrentStation.name;
        question.text = GameManager.Instance.CurrentQuestion.text;
        task.text = "Aufgabe " + currentQuestionCount + " / " + GameManager.Instance.CurrentStation.questionId.Count;
        questionPoints.text = GameManager.Instance.CurrentQuestion.points.ToString();
    }

    private void SetAnswerAndQuestionText()
    {
        if (GameManager.Instance.CurrentQuestion.type.Equals(QuestionType.choice))
        {
            choiceGameObject.SetActive(true);
            sequenceGameObject.SetActive(false);
            SetChoiceQuestionText();
            button1.interactable = true;
            button2.interactable = true;
            button3.interactable = true;
            button4.interactable = true;
        }

        if (GameManager.Instance.CurrentQuestion.type.Equals(QuestionType.sequence))
        {
            choiceGameObject.SetActive(false);
            sequenceGameObject.SetActive(true);
            SetSequenceButtons();
        }
    }

    private void SetChoiceQuestionText()
    {
        question.text = GameManager.Instance.CurrentQuestion.text;
        
        // Answer 1
        if(GameManager.Instance.CurrentAnswers.Count >= 1)
            answer1.text = GameManager.Instance.CurrentAnswers[0].text;
        
        // Answer 2
        if(GameManager.Instance.CurrentAnswers.Count >= 2)
            answer2.text = GameManager.Instance.CurrentAnswers[1].text;
        
        // Answer 3
        button3.interactable = GameManager.Instance.CurrentAnswers.Count >= 3;
        buttonImage3.enabled = GameManager.Instance.CurrentAnswers.Count >= 3;
        answer3.enabled = GameManager.Instance.CurrentAnswers.Count >= 3;
        if (GameManager.Instance.CurrentAnswers.Count >= 3)
            answer3.text = GameManager.Instance.CurrentAnswers[2].text;

        // Answer 4
        button4.interactable = GameManager.Instance.CurrentAnswers.Count >= 4;
        buttonImage4.enabled = GameManager.Instance.CurrentAnswers.Count >= 4;
        answer4.enabled = GameManager.Instance.CurrentAnswers.Count >= 4;
        if (GameManager.Instance.CurrentAnswers.Count >= 4)
            answer4.text = GameManager.Instance.CurrentAnswers[3].text;
    }

    private void SetSequenceButtons()
    {
        int offset = 0;
        float contentSize = 0;
        
        foreach (Answer answer in GameManager.Instance.CurrentAnswers)
        {
            GameObject sequenceGameObject = Instantiate(sequenceItem, transform.position, quaternion.identity);
            SequenceItemInterface sequenceItemInterface = sequenceGameObject.GetComponent<SequenceItemInterface>();
            sequenceItemInterface.AnswerTMP.SetText(answer.text);
            
            sequenceObjects.Add(sequenceGameObject);
            numberTMPList.Add(sequenceItemInterface.NumberTMP, -1);
            sequenceImages.Add(sequenceItemInterface.SequenceImage);
            sequenceButtons.Add(sequenceItemInterface.Button);

            sequenceGameObject.transform.parent = sequenceItemParent;
            sequenceGameObject.transform.localPosition = new Vector3(0, offset, 0);
            sequenceGameObject.transform.localScale = Vector3.one;
            offset -= 60;
            contentSize += 60;
            
            sequenceItemInterface.Button.onClick.AddListener(() =>
            {
                ClickedOnSequence(sequenceItemInterface.NumberTMP, sequenceItemInterface.Button);
            });
        }
        sequenceContentTransform.sizeDelta = new Vector2(0, contentSize + 30);
    }

    private void ClickedOnSequence(TextMeshProUGUI TMP, Button button)
    {
        int numberToShow = sequenceCounter + 1;
        TMP.SetText(numberToShow.ToString());
        numberTMPList[TMP] = sequenceCounter;
        button.interactable = false;
        sequenceCounter++;
    }

    private void UpdateCurrentQuestionDataTexts()
    {
        currentQuestionCount++;
        task.text = "Aufgabe " + currentQuestionCount + " / " + GameManager.Instance.CurrentStation.questionId.Count;
        questionPoints.text = GameManager.Instance.CurrentQuestion.points.ToString();
    }

    private void LoadNewQuestion()
    {
        if (GameManager.Instance.SetNextQuestion() == false)
        {
            SceneManager.LoadEndScreen();
        }
        
        SetAnswerAndQuestionText();
        MakeButtonGrey();
        tipScreen.ResetTip();
        UpdateCurrentQuestionDataTexts();
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

    private void ShowIfCorrect(Image button, bool correct)
    {
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;
        
        if(correct)
            button.sprite = green;

        if (!correct)
        {
            button.sprite = red;
            for(int i = 0; i < GameManager.Instance.CurrentAnswers.Count; i++)
            {
                if (CheckAnswer(i))
                    answerButtons[i].GetComponent<Image>().sprite = green;
            }
        }
    }

    private void MakeButtonGrey()
    {
        buttonImage1.sprite = purple;
        buttonImage2.sprite = purple;
        buttonImage3.sprite = purple;
        buttonImage4.sprite = purple;
    }

    public void ShowNextQuestion()
    {
        sequenceObjects.Clear();
        numberTMPList.Clear();
        sequenceImages.Clear();
        
        for (int i = 0; i < sequenceObjects.Count; i++)
        {
            GameObject sequence = sequenceObjects[i];
            sequenceButtons[i].onClick.RemoveAllListeners();
            sequenceObjects.Remove(sequence);
            Destroy(sequence);
        }

        LoadNewQuestion();
    }

    private void SetButtonMethods()
    {
        for (int i = 0; i < answerButtons.Count; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() =>
            {
                GameManager.Instance.AdChosenAnswer(currentQuestionCount, index, CheckAnswer(index));
                ShowIfCorrect(answerButtons[index].GetComponent<Image>(), GameManager.Instance.CurrentAnswers[index].isCorrect);
                tipScreen.ShowTipButton(GameManager.Instance.CurrentHint != null);
            });
        }
    }
}
