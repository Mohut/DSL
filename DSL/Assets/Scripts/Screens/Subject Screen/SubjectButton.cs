using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SubjectButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputField;
    
    [Header("Buttons")]
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<TextMeshProUGUI> buttonTexts;
    [SerializeField] private Button homebutton;

    private void Start()
    {
        SetUpButtons();
        homebutton.onClick.AddListener(SceneManager.LoadMainMenu);
    }

    private void SetUpButtons()
    {
        for (int i = 0; i < DataManager.Instance.Stations.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttonTexts[i].SetText(DataManager.Instance.Stations[i].name);
            Station buttonStation = DataManager.Instance.Stations[i];
            buttons[i].onClick.AddListener(() =>
            {
                DataManager.Instance.AddNewGroup(inputField.text);
                GameManager.Instance.SetCurrentStation(buttonStation);
                SceneManager.LoadQuestionScreen();
            });
        }
    }

    private void OnDisable()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
        homebutton.onClick.RemoveAllListeners();
    }
}
