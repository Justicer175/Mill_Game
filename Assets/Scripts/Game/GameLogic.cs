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
    public TMP_Text instructions;
    [SerializeField] private WinnerPopupScript winnerScript;

    [Header("P1 elements")]
    [SerializeField] private TMP_Text p1Name;
    [SerializeField] private TMP_Text p1NumberOfPieces;
    [SerializeField] private Image p1PiecesColor;
    [HideInInspector] public List<PositionInGame> p1Positions = new List<PositionInGame>();

    [Header("P2 elemnts")]
    [SerializeField] private TMP_Text p2Name;
    [SerializeField] private TMP_Text p2NumberOfPieces;
    [SerializeField] private Image p2PiecesColor;
    [HideInInspector] public List<PositionInGame> p2Positions = new List<PositionInGame>();

    [Header("Texts")]
    [SerializeField] private string placePiece = "is placing the piece";
    [SerializeField] private string movePiece = "is moving the piece";
    [SerializeField] private string mill = "Mill for player ";
    [SerializeField]private string removePiece = ", please remove the piece from player";
    [SerializeField] private string jumping = "is moving the piece with jumping.";

    [Header("Speed of piece moving")]
    [SerializeField] private int frameToMoveOn = 10;

    //last player that removed the piece
    private PositionInGame.Players lastPlayer = PositionInGame.Players.unused;
    [HideInInspector] public PositionInGame.Players getLastPlayer { get => lastPlayer; }
    [HideInInspector] public int numberOfPiecesPlaced = 0;
    [HideInInspector] public bool gameEnded = false;

    //color objects
    private List<PositionInGame> usedPositions = new List<PositionInGame>();


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
        //reset all the values on game start
        PlacingPiece(PositionInGame.Players.p1);
        p1Name.text = GLOBAL.instance.p1Name;
        p2Name.text = GLOBAL.instance.p2Name;
        p1NumberOfPieces.text = GLOBAL.instance.numberOfPieces.ToString();
        p2NumberOfPieces.text = GLOBAL.instance.numberOfPieces.ToString();
        numberOfPiecesPlaced = 0;
        p1Positions = new List<PositionInGame>();
        p2Positions = new List<PositionInGame>();
        gameEnded = false;

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

    public void RemoveGamePiece(PositionInGame.Players player)
    {
        if(player == PositionInGame.Players.p1)
        {
            instructions.text = (mill + GLOBAL.instance.p1Name + removePiece + " " + GLOBAL.instance.p2Name);
            lastPlayer = PositionInGame.Players.p1;
        }
        else if(player == PositionInGame.Players.p2)
        {
            instructions.text = (mill + GLOBAL.instance.p2Name + removePiece + " " + GLOBAL.instance.p1Name);
            lastPlayer = PositionInGame.Players.p2;
        }
        else
        {
            Debug.Log("Error getting player on remove");
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

    public void ShowPossiblePositions(PositionInGame pig)
    {
        usedPositions = new List<PositionInGame>();

        int layer = pig.GetLayer();
        switch (pig.GetPositionInLogic().x, pig.GetPositionInLogic().y)
        {
            case (0, 0):
                if (GLOBAL.instance.allPositionsArray[0, 1, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[0, 1, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[0, 1, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[1, 0, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 0, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 0, layer]);
                }
                break;
            case (0, 1):
                if (GLOBAL.instance.allPositionsArray[0, 0, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[0, 0, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[0, 0, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[0, 2, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[0, 2, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[0, 2, layer]);
                }
                //down checker
                if (layer > 0 && GLOBAL.instance.allPositionsArray[0, 1, layer-1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[0, 1, layer-1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[0, 1, layer-1]);
                }
                //up checker
                if (layer < GLOBAL.instance.numberOfSquares-1 && GLOBAL.instance.allPositionsArray[0, 1, layer + 1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[0, 1, layer + 1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[0, 1, layer + 1]);
                }
                break;
            case (0, 2):
                if (GLOBAL.instance.allPositionsArray[1, 2, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 2, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 2, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[1, 0, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 0, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 0, layer]);
                }
                break;
            case (1, 0):
                if (GLOBAL.instance.allPositionsArray[0, 0, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[0, 0, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[0, 0, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[2, 0, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 0, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 0, layer]);
                }
                //down checker
                if (layer > 0 && GLOBAL.instance.allPositionsArray[1, 0, layer - 1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 0, layer - 1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 0, layer - 1]);
                }
                //up checker
                if (layer < GLOBAL.instance.numberOfSquares - 1 && GLOBAL.instance.allPositionsArray[1, 0, layer + 1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 0, layer + 1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 0, layer + 1]);
                }
                break;
            case (1, 1):
                //doesnt exist
                break;
            case (1, 2):
                if (GLOBAL.instance.allPositionsArray[0, 2, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[0, 2, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[0, 2, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[2, 2, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 2, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 2, layer]);
                }
                //down checker
                if (layer > 0 && GLOBAL.instance.allPositionsArray[1, 2, layer - 1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 2, layer - 1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 2, layer - 1]);
                }
                //up checker
                if (layer < GLOBAL.instance.numberOfSquares - 1 && GLOBAL.instance.allPositionsArray[1, 2, layer + 1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 2, layer + 1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 2, layer + 1]);
                }
                break;
            case (2, 0):
                if (GLOBAL.instance.allPositionsArray[1, 0, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 0, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 0, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[2, 1, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 1, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 1, layer]);
                }
                break;
            case (2, 1):
                if (GLOBAL.instance.allPositionsArray[2, 0, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 0, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 0, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[2, 2, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 2, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 2, layer]);
                }
                //down checker
                if (layer > 0 && GLOBAL.instance.allPositionsArray[2, 1, layer - 1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 1, layer - 1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 1, layer - 1]);
                }
                //up checker
                if (layer < GLOBAL.instance.numberOfSquares - 1 && GLOBAL.instance.allPositionsArray[2, 1, layer + 1].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 1, layer + 1].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 1, layer + 1]);
                }
                break;
            case (2, 2):
                if (GLOBAL.instance.allPositionsArray[1, 2, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[1, 2, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[1, 2, layer]);
                }
                if (GLOBAL.instance.allPositionsArray[2, 1, layer].GetPlayer() == PositionInGame.Players.unused)
                {
                    GLOBAL.instance.allPositionsArray[2, 1, layer].GetBackgroundColor().SetActive(true);
                    usedPositions.Add(GLOBAL.instance.allPositionsArray[2, 1, layer]);
                }
                break;

        }
    }

    public void RemovePossiblePositions()
    {
        foreach(PositionInGame pig in usedPositions)
        {
            pig.GetBackgroundColor().SetActive(false);
        }
    }

    public void MovePiece(PositionInGame pigStart, PositionInGame pigEnd)
    {
        //move
        pigStart.GetPieceOnPoisiton().transform.position = pigEnd.GetPositionInGame();

        //new point
        pigEnd.SetPlayer(pigStart.GetPlayer());
        pigEnd.SetPieceOnPoisiton(pigStart.GetPieceOnPoisiton());
        lastPlayer = pigEnd.GetPlayer();

        //remove from list of points
        if (lastPlayer == PositionInGame.Players.p1)
        {
            MovePieceSetText(PositionInGame.Players.p2);
            CheckForWinner(PositionInGame.Players.p2);
            foreach (PositionInGame pig in p1Positions)
            {
                if(pig.GetPositionInLogic() == pigStart.GetPositionInLogic())
                {
                    p1Positions.Remove(pig);
                    p1Positions.Add(pigEnd);
                    break;
                }
            }
        }
        else if (lastPlayer == PositionInGame.Players.p2)
        {
            MovePieceSetText(PositionInGame.Players.p1);
            CheckForWinner(PositionInGame.Players.p1);
            foreach (PositionInGame pig in p2Positions)
            {
                if (pig.GetPositionInLogic() == pigStart.GetPositionInLogic())
                {
                    p2Positions.Remove(pig);
                    p2Positions.Add(pigEnd);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Error getting palyers");
        }

        //remove old save
        pigStart.SetPlayer(PositionInGame.Players.unused);
        pigStart.SetPieceOnPoisiton(null);

        CheckForMill(pigEnd.GetPlayer(), pigEnd.GetPositionInLogic(),pigEnd.GetLayer());
    }

    public void PlacingPiece(PositionInGame.Players player)
    {
        if(player == PositionInGame.Players.p1)
        {
            instructions.text = GLOBAL.instance.p1Name + " " + placePiece;
            lastPlayer = PositionInGame.Players.p1;
        }
        else if(player == PositionInGame.Players.p2)
        {
            instructions.text = GLOBAL.instance.p2Name + " " + placePiece;
            lastPlayer = PositionInGame.Players.p2;
        }
        else
        {
            Debug.Log("Error finding player");
        }
    }

    public void MovePieceSetText(PositionInGame.Players player)
    {
        if (player == PositionInGame.Players.p1)
        {
            instructions.text = GLOBAL.instance.p1Name + " " + movePiece;
            lastPlayer = PositionInGame.Players.p1;
        }
        else if (player == PositionInGame.Players.p2)
        {
            instructions.text = GLOBAL.instance.p2Name + " " + movePiece;
            lastPlayer = PositionInGame.Players.p2;
        }
        else
        {
            Debug.Log("Error finding player");
        }
    }

    public void CheckForWinner(PositionInGame.Players player)
    {
        //Debug.Log(GLOBAL.instance.getGameLogic.numberOfPiecesPlaced);
        //Debug.Log(GLOBAL.instance.numberOfPieces);
        if (GLOBAL.instance.getGameLogic.numberOfPiecesPlaced != GLOBAL.instance.numberOfPieces) return;

        if (player == PositionInGame.Players.p1)
        {
            if(p1Positions.Count <= 2)
            {
                gameEnded = true;
                winnerScript.ActivatePopup(WinnerPopupScript.Win.NoPieces, PositionInGame.Players.p2);
            }
            else if(!ImpossibleMoves(p1Positions))
            {
                gameEnded = true;
                winnerScript.ActivatePopup(WinnerPopupScript.Win.NoMoves, PositionInGame.Players.p2);
            }
        }
        else if(player == PositionInGame.Players.p2)
        {
            if (p2Positions.Count <= 2)
            {
                gameEnded = true;
                winnerScript.ActivatePopup(WinnerPopupScript.Win.NoPieces, PositionInGame.Players.p1);
            }
            else if (!ImpossibleMoves(p2Positions))
            {
                gameEnded = true;
                winnerScript.ActivatePopup(WinnerPopupScript.Win.NoMoves, PositionInGame.Players.p1);
            }
        }
        else
        {
            Debug.Log("Error getting player");
        }

    }

    private bool ImpossibleMoves(List<PositionInGame> positions)
    {
        bool canMove = false;
        foreach(PositionInGame pig in positions)
        {
            //write the logic behind it
            switch (pig.GetPositionInLogic().x, pig.GetPositionInLogic().y)
            {
                case (0, 0):
                    if (GLOBAL.instance.allPositionsArray[0, 1, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    if (GLOBAL.instance.allPositionsArray[1, 0, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    break;
                case (0, 1):
                    if (GLOBAL.instance.allPositionsArray[0, 0, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    if (GLOBAL.instance.allPositionsArray[0, 2, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    //down checker
                    if (pig.GetLayer() > 0 && GLOBAL.instance.allPositionsArray[0, 1, pig.GetLayer() - 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    //up checker
                    if (pig.GetLayer() < GLOBAL.instance.numberOfSquares - 1 && GLOBAL.instance.allPositionsArray[0, 1, pig.GetLayer() + 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    break;
                case (0, 2):
                    if (GLOBAL.instance.allPositionsArray[1, 2, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    if (GLOBAL.instance.allPositionsArray[1, 0, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    break;
                case (1, 0):
                    if (GLOBAL.instance.allPositionsArray[0, 0, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    if (GLOBAL.instance.allPositionsArray[2, 0, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    //down checker
                    if (pig.GetLayer() > 0 && GLOBAL.instance.allPositionsArray[1, 0, pig.GetLayer() - 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    //up checker
                    if (pig.GetLayer() < GLOBAL.instance.numberOfSquares - 1 && GLOBAL.instance.allPositionsArray[1, 0, pig.GetLayer() + 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    break;
                case (1, 1):
                    //doesnt exist
                    break;
                case (1, 2):
                    if (GLOBAL.instance.allPositionsArray[0, 2, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    if (GLOBAL.instance.allPositionsArray[2, 2, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    //down checker
                    if (pig.GetLayer() > 0 && GLOBAL.instance.allPositionsArray[1, 2, pig.GetLayer() - 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    //up checker
                    if (pig.GetLayer() < GLOBAL.instance.numberOfSquares - 1 && GLOBAL.instance.allPositionsArray[1, 2, pig.GetLayer() + 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    break;
                case (2, 0):
                    if (GLOBAL.instance.allPositionsArray[1, 0, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    if (GLOBAL.instance.allPositionsArray[2, 1, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove; 
                    }
                    break;
                case (2, 1):
                    if (GLOBAL.instance.allPositionsArray[2, 0, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove; 
                    }
                    if (GLOBAL.instance.allPositionsArray[2, 2, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove; 
                    }
                    //down checker
                    if (pig.GetLayer() > 0 && GLOBAL.instance.allPositionsArray[2, 1, pig.GetLayer() - 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove; 
                    }
                    //up checker
                    if (pig.GetLayer() < GLOBAL.instance.numberOfSquares - 1 && GLOBAL.instance.allPositionsArray[2, 1, pig.GetLayer() + 1].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    break;
                case (2, 2):
                    if (GLOBAL.instance.allPositionsArray[1, 2, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    if (GLOBAL.instance.allPositionsArray[2, 1, pig.GetLayer()].GetPlayer() == PositionInGame.Players.unused)
                    {
                        canMove = true;
                        return canMove;
                    }
                    break;

            }

        }

        return canMove;
    }

}
