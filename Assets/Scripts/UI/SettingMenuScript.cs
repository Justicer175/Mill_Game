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

    [Header("Field")]
    [SerializeField] private TMP_InputField squares;
    [SerializeField] private TMP_InputField pieces;

    [Header("Default values")]
    [SerializeField] private int defaultNumberOfPieces = 9;
    [SerializeField] private int defaultNumberOfSquares = 3;
    [SerializeField] private string defaultP1Name = "White";
    [SerializeField] private string defaultP2Name = "Black";

    [SerializeField] private int numberOfPlacesPerSquare = 8;

    //
    private int changedValuePieces;
    private int changedValueSquares;
    
    
    //color stuff missing

    //[SerializeField] private

    // Start is called before the first frame update
    void Start()
    {
        changedValueSquares = defaultNumberOfSquares;
        changedValuePieces = defaultNumberOfPieces;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMenu()
    {
        if (CheckForName() && CheckForColor() && CheckForSquares() && CheckForPieces())
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
            Debug.Log(GLOBAL.instance.randomTest);
        }

    }

    private bool CheckForName()
    {
        //Debug.Log(p1Name.text.ToString() + " " + p2Name.text.ToString());
        if (p1Name.text.ToString() == p2Name.text.ToString())
        {
            popup.ActivatePopup(PopUpScript.Errors.SameName);
            return false;
        }
        else
        {
            return true;
        }

        //return p1Name.text.ToString() != p2Name.text.ToString();
    }

    private bool CheckForColor()
    {
        if (p1Color.value == p2Color.value)
        {
            popup.ActivatePopup(PopUpScript.Errors.SameColor);
            return false;
        }
        else
        {
            return true;
        }
        //return p1Color.value != p2Color.value;
    }

    private bool CheckForSquares()
    {
        changedValueSquares = int.Parse(squares.text);
        //int tmpSquares = int.Parse(squares.text);
        if(changedValueSquares <= 0)
        {
            popup.ActivatePopup(PopUpScript.Errors.ToLittleSquares);
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckForPieces()
    {
        changedValuePieces = int.Parse(pieces.text);
        if(changedValuePieces <= 2)
        {
            popup.ActivatePopup(PopUpScript.Errors.ToLittlePieces);
            return false;
        }
        else if ((changedValueSquares * 8) <= changedValuePieces)
        {
            popup.ActivatePopup(PopUpScript.Errors.ToManyPieces);
            return false;
        }
        else
        {
            return true;
        }
        //return 
    }
}
