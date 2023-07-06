using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string password;

    private void Start()
    {
        if (GameManager.Instance.SkipLogin)
        {
            ui.SetActive(false);
        }
    }

    public void CheckLogin()
    {
        if (inputField.text.Equals(password.ToLower()))
        {
            ui.SetActive(false);
            GameManager.Instance.SkipLogin = true;
        }
        else
        {
            errorMessage.SetActive(true);
            inputField.text = "";
        }
    }
}
