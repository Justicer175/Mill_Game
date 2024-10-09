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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        game.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitTheGame()
    {
        //SoundManager.Instance.PlaySFX(GLOBAL.instance.audioCollection.sfxBack);
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

    private void BackSettingsMenu()
    {
        //add if same name checker

    }
}
