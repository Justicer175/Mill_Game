using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{

    public enum StateOfGame
    {
        PlacingPieces,
        PlayingTheGame,
        EndOfGame,
        none
    }

    [Header("UI elements")]
    [Space(5.0f)]

    [Header("P1 elements")]
    [SerializeField] private TMP_Text p1Name;
    [SerializeField] private TMP_Text p1NumberOfPieces;
    [SerializeField] private Image p1PiecesColor;

    [Header("P2 elemnts")]
    [SerializeField] private TMP_Text p2Name;
    [SerializeField] private TMP_Text p2NumberOfPieces;
    [SerializeField] private Image p2PiecesColor;

    //game stuff
    // List<PositionInGame> allPositions;

    //private StateOfGame state = StateOfGame.none;


    // Update is called once per frame
    void Update()
    {
        switch (GLOBAL.instance.stateOfGame)
        {
            case StateOfGame.PlacingPieces:
                break;
            case StateOfGame.PlayingTheGame:
                break;
            case StateOfGame.EndOfGame:
                break;
            case StateOfGame.none:
                break;
        }
    }

    public void SetupField()
    {
        p1Name.text = GLOBAL.instance.p1Name;
        p2Name.text = GLOBAL.instance.p2Name;
        p1NumberOfPieces.text = GLOBAL.instance.numberOfPieces.ToString();
        p2NumberOfPieces.text = GLOBAL.instance.numberOfPieces.ToString();

        switch (GLOBAL.instance.p1Color)
        {
            case SettingMenuScript.Colors.white:
                p1PiecesColor.color = new Color(255, 255, 255);
                break;
            case SettingMenuScript.Colors.black:
                p1PiecesColor.color = new Color(0, 0, 0);
                break;
            case SettingMenuScript.Colors.red:
                p1PiecesColor.color = new Color(255, 0, 0);
                break;
            case SettingMenuScript.Colors.blue:
                p1PiecesColor.color = new Color(0, 0, 255);
                break;
        }

        switch (GLOBAL.instance.p2Color)
        {
            case SettingMenuScript.Colors.white:
                p2PiecesColor.color = new Color(255, 255, 255);
                break;
            case SettingMenuScript.Colors.black:
                p2PiecesColor.color = new Color(0, 0, 0);
                break;
            case SettingMenuScript.Colors.red:
                p2PiecesColor.color = new Color(255, 0, 0);
                break;
            case SettingMenuScript.Colors.blue:
                p2PiecesColor.color = new Color(0, 0, 255);
                break;
        }

        GLOBAL.instance.p1ActualColor = p1PiecesColor.color;
        GLOBAL.instance.p2ActualColor = p2PiecesColor.color;

        GLOBAL.instance.stateOfGame = StateOfGame.PlacingPieces;
    }

    public void SetupGameLists()
    {
        //for(int i = 0; i < GLOBAL.instance.numberOfSquares)
        //{
        //    for(int i)
        //}
    }
}
