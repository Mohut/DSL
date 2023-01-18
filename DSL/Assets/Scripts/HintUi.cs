using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintUi : MonoBehaviour
{
    [Header("References")]
    public Button closeButton;
    public Button showHintButton;
    public TMPro.TextMeshProUGUI tipText;

    // Wrapper Function to call PayForHint()
    private void OnEnable()
    {
        closeButton.onClick.AddListener(Close);
        showHintButton.onClick.AddListener(ShowHint);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Close);
        showHintButton.onClick.RemoveListener(ShowHint);
    }

    private void ShowHint()
    {
        GameManager.Instance.PayForHint();
        tipText.gameObject.SetActive(true);
        showHintButton.gameObject.SetActive(false);
        tipText.text = GameManager.Instance.CurrentHint.text;
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
