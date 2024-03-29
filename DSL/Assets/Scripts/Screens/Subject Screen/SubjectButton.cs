using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SubjectButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject subjectButtonPrefab;
    [SerializeField] private Transform parentTransform;
    
    [Header("Buttons")]
    [SerializeField] private List<TextMeshProUGUI> buttonTexts;
    [SerializeField] private Button homebutton;
    [SerializeField] private RectTransform contentTransform;

    private void Start()
    {
        CreateSubjectButtons();
        homebutton.onClick.AddListener(SceneManager.LoadMainMenu);

        if (string.IsNullOrEmpty(GameManager.Instance.CurrentGroup?.name))
            return;
        
        inputField.text = GameManager.Instance.CurrentGroup.name;
    }

    private void CreateSubjectButtons()
    {
        int yPosition = -25;
        float contentSize = contentTransform.rect.height;

        for (int i = 0; i < DataManager.Instance.Stations.Count; i++)
        {
            Vector3 buttonPosition = new Vector3(0, yPosition, 0);
            
            GameObject subjectButton = Instantiate(subjectButtonPrefab, parentTransform.position, quaternion.identity);
            subjectButton.transform.SetParent(parentTransform);
            subjectButton.GetComponent<SubjectButtonInterface>().RectTransform.localPosition = buttonPosition;
            subjectButton.GetComponent<SubjectButtonInterface>().SubjectTitleTMP.text = DataManager.Instance.Stations[i].name;
            
            int stationIndex = i;
            subjectButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                DataManager.Instance.AddNewGroup(inputField.text);
                DataManager.Instance.LastGroupName = inputField.text;
                Debug.Log(DataManager.Instance.LastGroupName);
                GameManager.Instance.SetCurrentStation(DataManager.Instance.Stations[stationIndex]);
                SceneManager.LoadQuestionScreen();
            });

            yPosition -= 60;
            contentSize += 60;
        }

        contentTransform.sizeDelta = new Vector2(0, contentSize + 30);
    }
}
