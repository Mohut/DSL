using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ResultAnswer : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private RectTransform contentTransform;
    [SerializeField] private Sprite red;
    [SerializeField] private Sprite green;

    private void Start()
    {
        FillInAnswers();
        GameManager.Instance.ChosenAnswers = new List<ChosenAnswer>();
        GameManager.Instance.AllUsedTips = 0;
    }

    private void FillInAnswers()
    {
        int space = -50;
        float contentSize = contentTransform.rect.height;
        foreach (ChosenAnswer answer in GameManager.Instance.ChosenAnswers)
        {
            CreateNewAnswer(answer, space);
            contentTransform.sizeDelta = new Vector2(0, contentSize);
            space -= 200;
            contentSize += 200;
        }
    }

    private void CreateNewAnswer(ChosenAnswer answer, int space)
    {
        GameObject answerObject = Instantiate(answerPrefab, parent.transform.position, quaternion.identity);
        answerObject.transform.SetParent(parent.transform);
        
        Vector3 spaceVector = new Vector3(0, space, 0);
        answerObject.transform.localPosition = spaceVector;
        
        FinishedQuestionInterface fqi = answerObject.GetComponent<FinishedQuestionInterface>();
        fqi.QuestionNumber.text = "Aufgabe: " + answer.TaskNumber;
        fqi.QuestionText.text = answer.Question.text;
        fqi.TipsUsed.text = "Tipps: " + answer.UsedTip;
        if (answer.Right)
        {
            answerObject.GetComponent<Image>().sprite = green;
        }
        else
        {
            answerObject.GetComponent<Image>().sprite = red;
        }
        answerObject.transform.localScale = Vector3.one;
    }
}
