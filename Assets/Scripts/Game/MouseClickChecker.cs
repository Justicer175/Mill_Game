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

    private bool p1Placing = true;
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

        }

        //Debug.Log(rayHit.collider.gameObject.GetComponent<PositionInGame>().GetPlayer());

        //Debug.Log(rayHit.collider.gameObject.name);
    }

    // Update is called once per frame
    private void SpawnaPiece(PositionInGame pig)
    {
        if(pig.GetPlayer() == PositionInGame.Players.unused)
        {
            if (p1Placing)
            {
                pig.SetPlayer(PositionInGame.Players.p1);
                pieceGO = Instantiate(piece, pig.GetPositionInGame(), Quaternion.identity);
                pieceGO.GetComponent<SpriteRenderer>().color = GLOBAL.instance.p1ActualColor;
                pieceGO.transform.parent = GLOBAL.instance.piecesHolderGOP1.transform;
                p1Placing = false;
                pig.SetPieceOnPoisiton(pieceGO);

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
                p1Placing = true;
                numberOfPiecesPlaced++;
                pig.SetPieceOnPoisiton(pieceGO);

                if (numberOfPiecesPlaced >= 2)
                {
                    GLOBAL.instance.getGameLogic.CheckForMill(PositionInGame.Players.p2, pig.GetPositionInLogic(),pig.GetLayer());
                }
            }

            if(numberOfPiecesPlaced == GLOBAL.instance.numberOfPieces)
            {
                GLOBAL.instance.previousStateOfGame = GameLogic.StateOfGame.MovingPieces;
                GLOBAL.instance.stateOfGame = GameLogic.StateOfGame.MovingPieces;
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
            Debug.Log("Remove piece");
            Debug.Log(pig.GetPieceOnPoisiton());

            GameObject piece = pig.GetPieceOnPoisiton();

            pig.SetPlayer(PositionInGame.Players.unused);

            if(piece != null){
                //piece.SetActive(false);
                Destroy(piece.gameObject);
                pig.SetPieceOnPoisiton(null);
                Debug.Log("Destroyed");
            }

            GLOBAL.instance.stateOfGame = GLOBAL.instance.previousStateOfGame;
        }
        else
        {
            Debug.Log("Error sound");
        }

    }
}
