using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SheetMenu : MonoBehaviour
{
    [SerializeField] private GameObject sheetButton;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private RectTransform contentTransform;
    private void Start()
    {
        LoadSheets();
    }

    private void LoadSheets()
    {
        int yPosition = -100;
        int contentSize = 150;
        
        foreach (KeyValuePair<string, string> sheetPair in DataManager.Instance.DataSheets)
        {
            GameObject button = Instantiate(sheetButton, new Vector3(0, yPosition, 0), quaternion.identity, parentTransform);
            SheetButtonInterface sheetButtonInterface = button.GetComponent<SheetButtonInterface>();
            contentTransform.sizeDelta = new Vector2(0, contentSize);
            button.GetComponent<RectTransform>().localPosition = new Vector3(0, yPosition,0);
            contentSize += 160;
            sheetButtonInterface.SheetName.SetText(sheetPair.Key);
            sheetButtonInterface.Button.onClick.AddListener(()=>
            {
                DataManager.Instance.Stations.Clear();
                DataManager.Instance.ReadCSVFile(sheetPair.Key);
                SceneManager.LoadSubjectScreen();
            });
            yPosition -= 150;
        }
    }
}