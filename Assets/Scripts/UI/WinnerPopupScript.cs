using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerPopupScript : MonoBehaviour
{
    [Header("Internal")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameWindow;
    [SerializeField] private TMP_Text textField;
    [HideInInspector] public PopUpScript.StatePopUp state;
    private RectTransform transformOfPopup;

    [Header("Animation going up")]
    [SerializeField] private int startingYofAnimation;
    [SerializeField] private int endingYofAnimation;
    [SerializeField] private int pixelsToMove;
    [SerializeField] private int frameToMoveOn;
    private int currentFrame = 0;

    [Header("Winner text")]
    [SerializeField] private string noMoreMoves = "is the WINNER because";
    [SerializeField] private string noMoreMoves2 = "can't make any more moves";
    [SerializeField] private string noMorePieces = "is the WINNE because ";
    [SerializeField] private string noMorePieces2 = "can't make a mill anymore";

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winnerSound;

    public enum Win
    {
        NoMoves,
        NoPieces,
    }

    private void Update()
    {
        //create animation going up
        switch (state)
        {
            case PopUpScript.StatePopUp.GoUp:
                PopupGoUp();
                break;
            case PopUpScript.StatePopUp.Wait:
                break;
            case PopUpScript.StatePopUp.GoDown:
                PopupGoDown();
                break;
            default:
                Debug.Log("Unknown state");
                break;
        }
    }

    public void ActivatePopup(Win win, PositionInGame.Players player)
    {
        audioSource.clip = winnerSound;
        audioSource.Play();

        gameObject.SetActive(true);
        transformOfPopup = gameObject.GetComponent<RectTransform>();
        state = PopUpScript.StatePopUp.GoUp;
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, startingYofAnimation);

        string winner = "";
        string loser = "";

        if(player == PositionInGame.Players.p1)
        {
            winner = GLOBAL.instance.p1Name;
            loser = GLOBAL.instance.p2Name;
        }
        else if (player == PositionInGame.Players.p2)
        {
            winner = GLOBAL.instance.p2Name;
            loser = GLOBAL.instance.p1Name;
        }
        else
        {
            Debug.Log("Error getting winner");
        }

        switch (win)
        {
            case Win.NoMoves:
                textField.text = winner + " " + noMoreMoves + " " + loser + " " + noMoreMoves2;
                break;
            case Win.NoPieces:
                textField.text = winner + " " + noMorePieces + " " + loser + " " + noMorePieces2;
                break;
            default:
                textField.text = "Not defined winner, Please define it in code";
                break;
        }


    }

    public void LeavePopUp()
    {
        //state = PopUpScript.StatePopUp.GoDown;
        GLOBAL.instance.pausedGame = false;
        GLOBAL.instance.DeleteGameField();
        gameWindow.SetActive(false);
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void PopupGoUp()
    {
        if (currentFrame >= frameToMoveOn)
        {
            currentFrame = 0;
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + pixelsToMove);
        }
        else
        {
            currentFrame++;
        }


        if (transformOfPopup.offsetMax.y >= endingYofAnimation)
        {
            state = PopUpScript.StatePopUp.Wait;
        }
    }

    private void PopupGoDown()
    {
        gameObject.SetActive(false);
    }
}
