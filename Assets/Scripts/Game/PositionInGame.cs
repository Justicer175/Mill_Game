using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInGame : MonoBehaviour
{
    public enum Players
    {
        p1,
        p2,
        unused
    }

    private Vector2 positionInGame = Vector2.zero;
    private Vector2 positionInLogic = Vector2.zero;
    private int layerPosition = 0;
    private Players player = Players.unused;

    public PositionInGame(Vector2 position )
    {
        positionInGame = position;
    }

    public Vector2 GetPositionInGame()
    {
        return positionInGame;
    }

    public Vector2 GetPositionInLogic()
    {
        return positionInLogic;
    }

    public Players GetPlayer()
    {
        return player;
    }
}
