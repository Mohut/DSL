using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TipScreen : MonoBehaviour
{
    [SerializeField] private Button tipButton;
    [SerializeField] private GameObject tipPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button showTippButton;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    public TMPro.TextMeshProUGUI pointsText;

    private void Start()
    {
        if (GameManager.Instance.CurrentHint != null)
            tipText.text = GameManager.Instance.CurrentHint.text;
        
        tipButton.onClick.AddListener(() => OpenTipWindow(true));
        closeButton.onClick.AddListener(() => OpenTipWindow(false));
        showTippButton.onClick.AddListener(ShowTip);
        ShowTipButton(GameManager.Instance.CurrentHint != null);
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
        
        if (GameManager.Instance.PaidForHint)
        {
            tipText.enabled = true;
            showTippButton.enabled = false;
        }
        else
        {
            tipText.enabled = false;
            showTippButton.enabled = true;
            showTippButton.gameObject.SetActive(true);
            pointsText.SetText("- " + GameManager.Instance.CurrentHint.price + " Punkte");
        }
    }

    public void ShowTipButton(bool state)
    {
        tipButton.enabled = state;
        tipButton.interactable = state;
    }

    private void ShowTip()
    {
        GameManager.Instance.UsedTips++;
        GameManager.Instance.AllUsedTips++;
        GameManager.Instance.PayForHint();
        currentScoreText.text = "Score: " + GameManager.Instance.CurrentGroup.points;
        tipText.enabled = true;
        showTippButton.enabled = false;
    }

    public void ResetTip()
    {
        GameManager.Instance.UsedTips = 0;
        GameManager.Instance.PaidForHint = false;
        if(GameManager.Instance.CurrentHint != null)
            tipText.text = GameManager.Instance.CurrentHint.text;
        tipText.enabled = false;
        showTippButton.enabled = true;
    }
}
