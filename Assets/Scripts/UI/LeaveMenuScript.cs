using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveMenuScript : MonoBehaviour
{

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameWindow;

    [Header("Animation going up")]
    [SerializeField] private int startingYofAnimation;
    [SerializeField] private int endingYofAnimation;
    [SerializeField] private int pixelsToMove;
    [SerializeField] private int frameToMoveOn;
    private int currentFrame = 0;
    private PopUpScript.StatePopUp state;

    private RectTransform transformOfLeaveMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PopUpScript.StatePopUp.GoUp:
                LeaveMenuPopUp();
                break;
            case PopUpScript.StatePopUp.Wait:
                break;
            case PopUpScript.StatePopUp.GoDown:
                ReturnToMainMenuButton();
                break;
            default:
                Debug.Log("Unknown state");
                break;
        }
    }


    public void ActivateLeaveMenu()
    {
        gameObject.SetActive(true);
        transformOfLeaveMenu = gameObject.GetComponent<RectTransform>();
        state = PopUpScript.StatePopUp.GoUp;
    }

    private void LeaveMenuPopUp()
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


        if (transformOfLeaveMenu.offsetMax.y >= endingYofAnimation)
        {
            state = PopUpScript.StatePopUp.Wait;
        }
    }

    public void ContinueButton()
    {
        GLOBAL.instance.pausedGame = false;
        gameObject.SetActive(false);
    }

    public void RestartButton()
    {
        gameObject.SetActive(false);
        GLOBAL.instance.pausedGame = false;
        GLOBAL.instance.DeleteGameField();
        GLOBAL.instance.CreateGameField();
    }

    public void ReturnToMainMenuButton()
    {
        GLOBAL.instance.pausedGame = false;
        GLOBAL.instance.DeleteGameField();
        gameWindow.SetActive(false);
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
