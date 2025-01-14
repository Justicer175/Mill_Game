using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingMenuScript : MonoBehaviour
{
    public enum Colors
    {
        white,
        black,
        red,
        blue
    }

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
    [SerializeField] private Slider volume;

    [Header("Default values")]
    [SerializeField] private int defaultNumberOfPieces = 9;
    [SerializeField] private int defaultNumberOfSquares = 3;
    [SerializeField] private string defaultP1Name = "White";
    [SerializeField] private string defaultP2Name = "Black";
    [SerializeField] private int numberOfPlacesPerSquare = 8;

    [SerializeField] private int defaultSizeOfCamera = 25;
    [SerializeField] private int distanceToAddOnNewRing = 5;

    private int changedValuePieces;
    private int changedValueSquares;
    
    void Start()
    {
        changedValueSquares = defaultNumberOfSquares;
        changedValuePieces = defaultNumberOfPieces;
    }



    public void ReturnToMenu()
    {
        if (CheckForName() && CheckForColor() && CheckForSquares() && CheckForPieces())
        {
            AdjustCamera();
            GLOBAL.instance.numberOfSquares = changedValueSquares;
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

    }

    private bool CheckForName()
    {
        if (p1Name.text.ToString() == p2Name.text.ToString())
        {
            popup.ActivatePopup(PopUpScript.Errors.SameName);
            return false;
        }
        else
        {
            GLOBAL.instance.p1Name = p1Name.text.ToString();
            GLOBAL.instance.p2Name = p2Name.text.ToString();
            return true;
        }
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
            switch (p1Color.value)
            {
                case 0:
                    GLOBAL.instance.p1Color = Colors.white;
                    break;
                case 1:
                    GLOBAL.instance.p1Color = Colors.black;
                    break;
                case 2:
                    GLOBAL.instance.p1Color = Colors.red;
                    break;
                case 3:
                    GLOBAL.instance.p1Color = Colors.blue;
                    break;
            }

            switch (p2Color.value)
            {
                case 0:
                    GLOBAL.instance.p2Color = Colors.white;
                    break;
                case 1:
                    GLOBAL.instance.p2Color = Colors.black;
                    break;
                case 2:
                    GLOBAL.instance.p2Color = Colors.red;
                    break;
                case 3:
                    GLOBAL.instance.p2Color = Colors.blue;
                    break;
            }
            return true;
        }
    }

    private bool CheckForSquares()
    {
        changedValueSquares = int.Parse(squares.text);
        if(changedValueSquares <= 0)
        {
            popup.ActivatePopup(PopUpScript.Errors.ToLittleSquares);
            return false;
        }
        else
        {
            GLOBAL.instance.numberOfSquares = changedValueSquares;
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
            GLOBAL.instance.numberOfPieces = changedValuePieces;
            return true;
        }
    }

    private void AdjustCamera()
    {
        if(changedValueSquares == defaultNumberOfSquares)
        {
            GLOBAL.instance.cameraSize = defaultSizeOfCamera;
        }
        else if(changedValueSquares < defaultNumberOfSquares && changedValueSquares > 0)
        {
            GLOBAL.instance.cameraSize = defaultSizeOfCamera - distanceToAddOnNewRing;
        }
        else if (changedValueSquares > defaultNumberOfSquares)
        {
            GLOBAL.instance.cameraSize = defaultSizeOfCamera + ((changedValueSquares - defaultNumberOfSquares) * distanceToAddOnNewRing);
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volume.value;
    }
}
