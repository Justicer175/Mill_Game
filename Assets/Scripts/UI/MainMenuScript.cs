using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        //play music
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void StartTheGame()
    {
        game.SetActive(true);
        mainMenu.SetActive(false);
        GLOBAL.instance.CreateGameField();
    }

    public void ExitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

}
