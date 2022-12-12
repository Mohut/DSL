using Unity.Mathematics;
using UnityEngine;

public class ResultAnswer : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject answerPrefab;

    private void Start()
    {
        foreach (ChosenAnswer answer in GameManager.Instance.ChosenAnswers)
        {
            CreateNewAnswer();
        }
    }

    private void CreateNewAnswer()
    {
        GameObject answer = Instantiate(answerPrefab, parent.transform.position, quaternion.identity);
        answer.transform.parent = parent.transform;
    }
}
