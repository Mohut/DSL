using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiplayerButton;
    void OnEnable()
    {
        multiplayerButton.onClick.AddListener(SceneManager.LoadSubjectScreen);
    }
}
