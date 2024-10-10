using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpScript : MonoBehaviour
{
    [Header("Internal")]
    [SerializeField] private TMP_Text textField;
    [HideInInspector] public StatePopUp state;
    private RectTransform transformOfPopup;

    [Header("Animation going up")]
    [SerializeField] private int startingYofAnimation;
    [SerializeField] private int endingYofAnimation;
    [SerializeField] private int pixelsToMove;
    [SerializeField] private int frameToMoveOn;
    private int currentFrame = 0;

    [Space (10.0f)]
    [Header("Errors")]
    [SerializeField] private string sameName;
    [SerializeField] private string sameColor;
    [SerializeField] private string toManyPieces;

    //public static final Integer field = 200;

    public enum Errors
    {
        SameName,
        SameColor,
        ToManyPieces,
        ToLittlePieces,
        ToLittleSquares,
    }

    public enum StatePopUp
    {
        GoUp,
        Wait,
        GoDown
    }

    private void Update()
    {
        //create animation going up
        switch (state)
        {
            case StatePopUp.GoUp:
                PopupGoUp();
                break;
            case StatePopUp.Wait:
                break;
            case StatePopUp.GoDown:
                PopupGoDown();
                break;
            default:
                Debug.Log("Unknown state");
                break;
        }
    }

    public void ActivatePopup(Errors error)
    {
        gameObject.SetActive(true);
        transformOfPopup = gameObject.GetComponent<RectTransform>();
        state = StatePopUp.GoUp;
        //Debug.Log(gameObject.transform.position.x + " "+ gameObject.transform.position.y);
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, startingYofAnimation);

        switch (error)
        { 
            case Errors.SameName:
                textField.text = sameName;
                break;
            case Errors.SameColor:
                textField.text = sameColor;
                break;
            case Errors.ToManyPieces:
                textField.text = toManyPieces;
                break;
            default:
                textField.text = "Not defined error, Please define it in code";
                break;
        }


    }

    public void LeavePopUp()
    {
        state = StatePopUp.GoDown;
    }

    private void PopupGoUp()
    {
        if(currentFrame >= frameToMoveOn)
        {
            currentFrame = 0;
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + pixelsToMove);
        }
        else
        {
            currentFrame++;
        }


        if(transformOfPopup.offsetMax.y >= endingYofAnimation)
        {
            state = StatePopUp.Wait;
        }
    }

    private void PopupGoDown()
    {
        gameObject.SetActive(false);
    }

}
