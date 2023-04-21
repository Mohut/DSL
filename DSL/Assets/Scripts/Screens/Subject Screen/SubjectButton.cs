using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SubjectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputField;
    [SerializeField] private GameObject subjectButtonPrefab;
    [SerializeField] private Transform parentTransform;
    
    [Header("Buttons")]
    [SerializeField] private List<TextMeshProUGUI> buttonTexts;
    [SerializeField] private Button homebutton;

    private void Start()
    {
        CreateSubjectButtons();
        homebutton.onClick.AddListener(SceneManager.LoadMainMenu);
    }

    private void CreateSubjectButtons()
    {
        int yPosition = -25;

        for (int i = 0; i < DataManager.Instance.Stations.Count; i++)
        {
            Vector3 buttonPosition = new Vector3(0, yPosition, 0);
            
            GameObject subjectButton = Instantiate(subjectButtonPrefab, parentTransform.position, quaternion.identity);
            subjectButton.transform.parent = parentTransform;
            subjectButton.GetComponent<SubjectButtonInterface>().RectTransform.localPosition = buttonPosition;
            subjectButton.GetComponent<SubjectButtonInterface>().SubjectTitleTMP.text = DataManager.Instance.Stations[i].name;
            
            int stationIndex = i;
            subjectButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                DataManager.Instance.AddNewGroup(inputField.text);
                GameManager.Instance.SetCurrentStation(DataManager.Instance.Stations[stationIndex]);
                SceneManager.LoadQuestionScreen();
            });

            yPosition -= 150;
        }
    }
}
