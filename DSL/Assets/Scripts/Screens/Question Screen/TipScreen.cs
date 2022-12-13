using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipScreen : MonoBehaviour
{
    [SerializeField] private Button tipButton;
    [SerializeField] private GameObject tipPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button showTippButton;
    [SerializeField] private TextMeshProUGUI tipText;

    private void Start()
    {
        tipText.text = GameManager.Instance.CurrentHint.text;
        tipButton.onClick.AddListener(() => OpenTipWindow(true));
        closeButton.onClick.AddListener(() => OpenTipWindow(false));
        showTippButton.onClick.AddListener(ShowTip);
    }

    private void OnDestroy()
    {
        tipButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
        showTippButton.onClick.RemoveAllListeners();
    }

    private void OpenTipWindow(bool state)
    {
        tipPanel.SetActive(state);
        if (GameManager.Instance.PayedForHint)
        {
            tipText.enabled = true;
            showTippButton.enabled = false;
        }
        else
        {
            tipText.enabled = false;
            showTippButton.enabled = true;
        }
    }

    private void ShowTip()
    {
        GameManager.Instance.UsedTips++;
        GameManager.Instance.PayForHint();
        tipText.enabled = true;
        showTippButton.enabled = false;
    }

    public void ResetTip()
    {
        GameManager.Instance.PayedForHint = false;
        tipText.text = GameManager.Instance.CurrentHint.text;
        tipText.enabled = false;
        showTippButton.enabled = true;
    }
}
