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

    public PositionInGame(Vector2 position, Vector2 position2, int layer, Players player)
    {
        positionInGame = position;
        positionInLogic = position2;
        layerPosition = layer;
        this.player = player;
    }

    public void SetAllTheValues(Vector2 position, Vector2 position2, int layer, Players player)
    {
        positionInGame = position;
        positionInLogic = position2;
        layerPosition = layer;
        this.player = player;
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

    public void SetPlayer(Players player)
    {
        this.player = player;
    }
}
