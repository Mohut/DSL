using Unity.Mathematics;
using UnityEngine;

public class ResultAnswer : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject answerPrefab;

    private void Start()
    {
        int space = -50;
        foreach (ChosenAnswer answer in GameManager.Instance.ChosenAnswers)
        {
            CreateNewAnswer(answer, space);
            space -= 150;
        }
    }

    private void CreateNewAnswer(ChosenAnswer answer, int space)
    {
        GameObject answerObject = Instantiate(answerPrefab, parent.transform.position, quaternion.identity);
        answerObject.transform.parent = parent.transform;
        
        Vector3 spaceVector = new Vector3(0, space, 0);
        answerObject.transform.localPosition = spaceVector;
        
        FinishedQuestionInterface fqi = answerObject.GetComponent<FinishedQuestionInterface>();
        fqi.QuestionNumber.text = answer.TaskNumber.ToString();
        fqi.QuestionText.text = answer.Question.text;
        fqi.TipsUsed.text = answer.UsedTip.ToString();
    }
}
