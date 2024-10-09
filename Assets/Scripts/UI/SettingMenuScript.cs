using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingMenuScript : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private PopUpScript popup;

    [Header("Player 1")]
    [SerializeField] private TMP_InputField p1Name;
    [SerializeField] private TMP_Dropdown p1Color;

    [Header("Player 2")]
    [SerializeField] private TMP_InputField p2Name;
    [SerializeField] private TMP_Dropdown p2Color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMenu()
    {
        popup.ActivatePopup(PopUpScript.Errors.SameColor);
        //if(CheckForName() && CheckForColor() && CheckForPieces())
        //{
        //    settingsMenu.SetActive(false);
        //    mainMenu.SetActive(true);
        //}
        
    }

    private void CheckForName()
    {

    }

    private void CheckForColor()
    {
        if(p1Color.value == p2Color.value)
        {

        }
    }

    private void CheckForPieces()
    {

    }
}
