using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class Endscreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI groupName;
    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private TextMeshProUGUI tipps;
    [SerializeField] private Button upperHomeButton;
    [SerializeField] private Button lowerHombeButton;

    private void Start()
    {
        groupName.text = GameManager.Instance.CurrentGroup.name;
        points.text = "Punkte: " + GameManager.Instance.CurrentGroup.points;
        tipps.text = "Tipps: " + GameManager.Instance.AllUsedTips;
        upperHomeButton.onClick.AddListener(SceneManager.LoadMainMenu);
        lowerHombeButton.onClick.AddListener(SceneManager.LoadMainMenu);

        DataManager.Instance.UploadSheet();
    }

    private void OnDisable()
    {
        upperHomeButton.onClick.RemoveAllListeners();
        lowerHombeButton.onClick.RemoveAllListeners();
    }
}
