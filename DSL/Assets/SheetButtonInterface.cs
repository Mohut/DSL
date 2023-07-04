using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SheetButtonInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sheetName;
    [SerializeField] private Button button;

    public TextMeshProUGUI SheetName { get => sheetName; set => sheetName = value; }
    public Button Button { get => button; set => button = value; }
}
