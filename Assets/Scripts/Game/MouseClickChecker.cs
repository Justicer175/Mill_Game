using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickChecker : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Prefab")]
    [SerializeField] private GameObject piece;
    private GameObject pieceGO;
    private PositionInGame pickedPiece;

    private bool p1Playing = true;
    private int numberOfPiecesPlaced = 0;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider) return;

        if (rayHit.collider.gameObject.GetComponent<PositionInGame>() == null) return;

        if(GLOBAL.instance.stateOfGame == GameLogic.StateOfGame.RemovingPieces)
        {
            RemovePiece(rayHit.collider.gameObject.GetComponent<PositionInGame>());
        }
        else if(GLOBAL.instance.stateOfGame == GameLogic.StateOfGame.PlacingPieces)
        {
            SpawnaPiece(rayHit.collider.gameObject.GetComponent<PositionInGame>());
        }
        else if(GLOBAL.instance.stateOfGame == GameLogic.StateOfGame.MovingPieces)
        {
            MovePiece(rayHit.collider.gameObject.GetComponent<PositionInGame>());
        }

        //Debug.Log(rayHit.collider.gameObject.GetComponent<PositionInGame>().GetPlayer());

        //Debug.Log(rayHit.collider.gameObject.name);
    }

    // Update is called once per frame
    private void SpawnaPiece(PositionInGame pig)
    {
        if(pig.GetPlayer() == PositionInGame.Players.unused)
        {
            if (p1Playing)
            {
                pig.SetPlayer(PositionInGame.Players.p1);
                pieceGO = Instantiate(piece, pig.GetPositionInGame(), Quaternion.identity);
                pieceGO.GetComponent<SpriteRenderer>().color = GLOBAL.instance.p1ActualColor;
                pieceGO.transform.parent = GLOBAL.instance.piecesHolderGOP1.transform;
                p1Playing = false;
                pig.SetPieceOnPoisiton(pieceGO);

                GLOBAL.instance.getGameLogic.PlacingPiece(PositionInGame.Players.p2);

                if (numberOfPiecesPlaced >= 2)
                {
                    GLOBAL.instance.getGameLogic.CheckForMill(PositionInGame.Players.p1, pig.GetPositionInLogic(), pig.GetLayer());
                }
            }
            else
            {
                pig.SetPlayer(PositionInGame.Players.p2);
                pieceGO = Instantiate(piece, pig.GetPositionInGame(), Quaternion.identity);
                pieceGO.GetComponent<SpriteRenderer>().color = GLOBAL.instance.p2ActualColor;
                pieceGO.transform.parent = GLOBAL.instance.piecesHolderGOP2.transform;
                p1Playing = true;
                numberOfPiecesPlaced++;
                pig.SetPieceOnPoisiton(pieceGO);

                GLOBAL.instance.getGameLogic.PlacingPiece(PositionInGame.Players.p1);

                if (numberOfPiecesPlaced == GLOBAL.instance.numberOfPieces)
                {
                    GLOBAL.instance.getGameLogic.MovePieceSetText(PositionInGame.Players.p1);
                    GLOBAL.instance.previousStateOfGame = GameLogic.StateOfGame.MovingPieces;
                    GLOBAL.instance.stateOfGame = GameLogic.StateOfGame.MovingPieces;
                }

                if (numberOfPiecesPlaced >= 2)
                {
                    GLOBAL.instance.getGameLogic.CheckForMill(PositionInGame.Players.p2, pig.GetPositionInLogic(),pig.GetLayer());
                }
            }

        }
        else
        {
            return;
        }
    }

    private void RemovePiece(PositionInGame pig)
    {

        if(GLOBAL.instance.getGameLogic.getLastPlayer != pig.GetPlayer() && pig.GetPlayer() != PositionInGame.Players.unused)
        {
            GameObject piece = pig.GetPieceOnPoisiton();

            pig.SetPlayer(PositionInGame.Players.unused);

            if(piece != null){
                Destroy(piece.gameObject);
                pig.SetPieceOnPoisiton(null);
            }

            GLOBAL.instance.stateOfGame = GLOBAL.instance.previousStateOfGame;

            if(GLOBAL.instance.previousStateOfGame == GameLogic.StateOfGame.PlacingPieces)
            {
                if(GLOBAL.instance.getGameLogic.getLastPlayer == PositionInGame.Players.p1)
                {
                    GLOBAL.instance.getGameLogic.PlacingPiece(PositionInGame.Players.p2);
                }
                else if(GLOBAL.instance.getGameLogic.getLastPlayer == PositionInGame.Players.p2)
                {
                    GLOBAL.instance.getGameLogic.PlacingPiece(PositionInGame.Players.p1);
                }
                else
                {
                    Debug.Log("Error finding last player");
                }
                
            }
            else if (GLOBAL.instance.previousStateOfGame == GameLogic.StateOfGame.MovingPieces)
            {
                if (GLOBAL.instance.getGameLogic.getLastPlayer == PositionInGame.Players.p1)
                {
                    GLOBAL.instance.getGameLogic.MovePieceSetText(PositionInGame.Players.p2);
                }
                else if (GLOBAL.instance.getGameLogic.getLastPlayer == PositionInGame.Players.p2)
                {
                    GLOBAL.instance.getGameLogic.MovePieceSetText(PositionInGame.Players.p1);
                }
                else
                {
                    Debug.Log("Error finding last player");
                }
            }
        }
        else
        {
            Debug.Log("Error sound");
        }

    }

    private void MovePiece(PositionInGame pig)
    {
        if(pickedPiece == null)
        {
            if(GLOBAL.instance.getGameLogic.getLastPlayer == pig.GetPlayer())
            {
                pickedPiece = pig;
                GLOBAL.instance.getGameLogic.ShowPossiblePositions(pig);
            }
        }
        else if (pickedPiece == pig)
        {
            pickedPiece = null;
            GLOBAL.instance.getGameLogic.RemovePossiblePositions();
        }
        else if (pig.GetBackgroundColor().activeSelf)
        {
            GLOBAL.instance.getGameLogic.MovePiece(pickedPiece, pig);
            GLOBAL.instance.getGameLogic.RemovePossiblePositions();
            pickedPiece = null;
        }
    }

}
