using TMPro;
using UnityEngine;

public class QuestionScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI groupName;
    void Start()
    {
        groupName.text = GameManager.Instance.CurrentGroup.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
