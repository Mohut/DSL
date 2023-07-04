using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SheetMenu : MonoBehaviour
{
    [SerializeField] private GameObject sheetButton;
    [SerializeField] private Transform parentTransform;
    private void Start()
    {
        LoadSheets();
    }

    private void LoadSheets()
    {
        int yPosition = 0;
        
        foreach (KeyValuePair<string, string> sheetPair in DataManager.Instance.DataSheets)
        {
            SheetButtonInterface sheetButtonInterface = Instantiate(sheetButton, new Vector3(0, yPosition, 0), quaternion.identity, parentTransform).GetComponent<SheetButtonInterface>();
            sheetButtonInterface.SheetName.SetText(sheetPair.Key);
            sheetButtonInterface.Button.onClick.AddListener(()=>
            {
                DataManager.Instance.Stations.Clear();
                DataManager.Instance.ReadCSVFile(sheetPair.Key);
                SceneManager.LoadSubjectScreen();
            });
            yPosition += 200;
        }
    }
}
