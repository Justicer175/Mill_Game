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
        MovingPieces,
        RemovingPieces,
        EndOfGame,
        none
    }

    [Header("UI elements")]
    [Space(5.0f)]
    public TMP_Text instructions;

    [Header("P1 elements")]
    [SerializeField] private TMP_Text p1Name;
    [SerializeField] private TMP_Text p1NumberOfPieces;
    [SerializeField] private Image p1PiecesColor;
    [SerializeField] private List<PositionInGame> p1Positions = new List<PositionInGame>();

    [Header("P2 elemnts")]
    [SerializeField] private TMP_Text p2Name;
    [SerializeField] private TMP_Text p2NumberOfPieces;
    [SerializeField] private Image p2PiecesColor;
    [SerializeField] private List<PositionInGame> p2Positions = new List<PositionInGame>();

    [Header("Texts")]
    [SerializeField] private string placePiece = " is placing the piece";
    [SerializeField] private string movePiece = " is moving the piece";
    [SerializeField] private string mill = "Mill for player ";
    [SerializeField]private string removePiece = ", please remove the piece from player";
    [SerializeField] private string jumping = " is moving the piece with jumping.";

    //last player that removed the piece
    private PositionInGame.Players lastPlayer = PositionInGame.Players.unused;
    [HideInInspector] public PositionInGame.Players getLastPlayer { get => lastPlayer; }

    // Update is called once per frame
    void Update()
    {
        switch (GLOBAL.instance.stateOfGame)
        {
            case StateOfGame.PlacingPieces:
                break;
            case StateOfGame.MovingPieces:
                break;
            case StateOfGame.RemovingPieces:
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

        GLOBAL.instance.previousStateOfGame = StateOfGame.PlacingPieces;
        GLOBAL.instance.stateOfGame = StateOfGame.PlacingPieces;
    }

    public void SetupGameLists()
    {
        //for(int i = 0; i < GLOBAL.instance.numberOfSquares)
        //{
        //    for(int i)
        //}
    }

    public void RemoveGamePiece(PositionInGame.Players player)
    {
        if(player == PositionInGame.Players.p1)
        {
            instructions.text = (mill + GLOBAL.instance.p1Name + removePiece + " " + GLOBAL.instance.p2Name);
            lastPlayer = PositionInGame.Players.p1;
        }
        else
        {
            instructions.text = (mill + GLOBAL.instance.p2Name + removePiece + " " + GLOBAL.instance.p1Name);
            lastPlayer = PositionInGame.Players.p2;
        }

        GLOBAL.instance.stateOfGame = StateOfGame.RemovingPieces;
    }

    public void CheckForMill(PositionInGame.Players player, Vector2 posOfPiece, int layer)
    {

        switch ((posOfPiece.x, posOfPiece.y))
        {
            case (0, 0):
                if (GLOBAL.instance.allPositionsArray[1, 0, layer].GetPlayer() == player &&
                   GLOBAL.instance.allPositionsArray[2, 0, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                else if (GLOBAL.instance.allPositionsArray[0, 1, layer].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[0, 2, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                break;
            case (0, 1):
                if (GLOBAL.instance.allPositionsArray[0, 0, layer].GetPlayer() == player &&
                        GLOBAL.instance.allPositionsArray[0, 2, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                    break;
                }

                //add line checker
                //check up and down
                if (layer != 0 && layer != GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[0, 1, layer-1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[0, 1, layer+1].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check down
                if (layer - 2 >= 0)
                {
                    if (GLOBAL.instance.allPositionsArray[0, 1, layer - 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[0, 1, layer - 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check up
                if(layer + 2 <= GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[0, 1, layer + 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[0, 1, layer + 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                break;
            case (0, 2):
                if (GLOBAL.instance.allPositionsArray[0, 0, layer].GetPlayer() == player &&
                        GLOBAL.instance.allPositionsArray[0, 1, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                else if (GLOBAL.instance.allPositionsArray[1, 2, layer].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[2, 2, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                break;
            case (1, 0):
                if (GLOBAL.instance.allPositionsArray[0, 0, layer].GetPlayer() == player &&
                        GLOBAL.instance.allPositionsArray[2, 0, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                    break;
                }
                //add line checker

                //check up and down
                if (layer != 0 && layer != GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[1, 0, layer - 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[1, 0, layer + 1].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check down
                if (layer - 2 >= 0)
                {
                    if (GLOBAL.instance.allPositionsArray[1, 0, layer - 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[1, 0, layer - 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check up
                if (layer + 2 <= GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[1, 0, layer + 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[1, 0, layer + 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                break;
            case (1, 1):
                //does not exisit
                break;
            case (1, 2):
                if (GLOBAL.instance.allPositionsArray[0, 2, layer].GetPlayer() == player &&
                        GLOBAL.instance.allPositionsArray[2, 2, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                    break;
                }
                //add line checker

                //check up and down
                if (layer != 0 && layer != GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[1, 2, layer - 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[1, 2, layer + 1].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check down
                if (layer - 2 >= 0)
                {
                    if (GLOBAL.instance.allPositionsArray[1, 2, layer - 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[1, 2, layer - 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check up
                if (layer + 2 <= GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[1, 2, layer + 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[1, 2, layer + 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                break;
            case (2, 0):
                if (GLOBAL.instance.allPositionsArray[0, 0, layer].GetPlayer() == player &&
                        GLOBAL.instance.allPositionsArray[1, 0, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                else if (GLOBAL.instance.allPositionsArray[2, 1, layer].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[2, 2, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                break;
            case (2, 1):
                if (GLOBAL.instance.allPositionsArray[2, 0, layer].GetPlayer() == player &&
                        GLOBAL.instance.allPositionsArray[2, 2, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                    break;
                }
                //add line checker

                //check up and down
                if (layer != 0 && layer != GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[2, 1, layer - 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[2, 1, layer + 1].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check down
                if (layer - 2 >= 0)
                {
                    if (GLOBAL.instance.allPositionsArray[2, 1, layer - 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[2, 1, layer - 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                //check up
                if (layer + 2 <= GLOBAL.instance.numberOfSquares - 1)
                {
                    if (GLOBAL.instance.allPositionsArray[2, 1, layer + 1].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[2, 1, layer + 2].GetPlayer() == player)
                    {
                        RemoveGamePiece(player);
                        break;
                    }
                }
                break;
            case (2, 2):
                if (GLOBAL.instance.allPositionsArray[2, 0, layer].GetPlayer() == player &&
                        GLOBAL.instance.allPositionsArray[2, 1, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                else if (GLOBAL.instance.allPositionsArray[0, 2, layer].GetPlayer() == player &&
                            GLOBAL.instance.allPositionsArray[1, 2, layer].GetPlayer() == player)
                {
                    RemoveGamePiece(player);
                }
                break;
            default:
                break;
        }

    }
}
