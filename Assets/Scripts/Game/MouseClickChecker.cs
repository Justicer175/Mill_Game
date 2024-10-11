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

        if(GLOBAL.instance.stateOfGame == GameLogic.StateOfGame.PlacingPieces)
        {
            SpawnaPiece(rayHit.collider.gameObject.GetComponent<PositionInGame>());
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
                p1Placing = false;
            }
            else
            {
                pig.SetPlayer(PositionInGame.Players.p2);
                pieceGO = Instantiate(piece, pig.GetPositionInGame(), Quaternion.identity);
                pieceGO.GetComponent<SpriteRenderer>().color = GLOBAL.instance.p2ActualColor;
                p1Placing = true;
                numberOfPiecesPlaced++;
            }

            if(numberOfPiecesPlaced == GLOBAL.instance.numberOfPieces)
            {
                GLOBAL.instance.stateOfGame = GameLogic.StateOfGame.PlayingTheGame;
            }

        }
        else
        {
            return;
        }
    }
}
