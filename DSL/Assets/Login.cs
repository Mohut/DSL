using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string password;
    
    public void CheckLogin()
    {
        if (inputField.text.Equals(password.ToLower()))
        {
            ui.SetActive(false);
        }
        else
        {
            errorMessage.SetActive(true);
            inputField.text = "";
        }
    }
}
