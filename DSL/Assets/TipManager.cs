using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour
{
    [SerializeField] private GameObject tipUI;
    

    public void ShowTipUI()
    {
        tipUI.SetActive(true);
    }

    public void ShowTip()
    {
        
    }
}
