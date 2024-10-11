using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLOBAL : MonoBehaviour
{
    public static GLOBAL instance;

    [Header("Internal")]
    [SerializeField] private GameObject pointHolderGO;
    [SerializeField] private GameObject lineHolderGO;
    [SerializeField] private GameObject piecesHolderGOP1;
    [SerializeField] private GameObject piecesHolderGOP2;
    [SerializeField] private LeaveMenuScript pauseMenu;

    [Header("Prefabs")]
    [SerializeField] private GameObject piece;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject point;

    [Header("Game config")]
    [SerializeField] private float distanceLeftRight = 5;
    [SerializeField] private float distanceTopBottom = 5f;
    [SerializeField] private float layerOfLine = 0;
    [SerializeField] private float layerOfPoint = 1;
    [SerializeField] private float layerOfPiece = 2;

    [Space(5.0f)]
    [SerializeField] private float distanceLeftRightLine = 2.5f;
    [SerializeField] private float distanceTopBotLine = 2.5f;
    [SerializeField] private float distanceBetweenSquares = 5;
    [SerializeField] private Vector2 lineHorizontalScale = new Vector2(5.5f,0.5f);
    [SerializeField] private Vector2 lineVerticalScale = new Vector2(0.5f,5.5f);

    private Vector3 positionOfGO = Vector3.zero;
    private GameObject pointGO;
    private GameObject lineGO;


    [System.NonSerialized] public int numberOfSquares = 3;
    [System.NonSerialized] public int numberOfPieces = 9;
    [System.NonSerialized] public int numberOfPointsPerSquare = 9;
    [System.NonSerialized] public int numberOfLinesPerSquare = 8;
    [System.NonSerialized] public int numberOfExtraLines = 2;
    [System.NonSerialized] public int numberOfConnectionsBetweenSquares = 4;

    //Check if the game is running
    [System.NonSerialized] public bool gameRunning = false;




    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameRunning && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.ActivateLeaveMenu();
        }
    }


    public void CreateGameField()
    {
        gameRunning = true;
        for (int currentSquare = 0; currentSquare < numberOfSquares; currentSquare++)
        {
            //spawn point
            for (int currentPoint = 0; currentPoint < numberOfPointsPerSquare; currentPoint++)
            {
                //position calculator
                switch (currentPoint)
                {
                    case 0:
                        CreatePoint(-1, 1, currentSquare, layerOfPoint);
                        break;
                    case 1:
                        CreatePoint(0, 1, currentSquare, layerOfPoint);
                        break;
                    case 2:
                        CreatePoint(1, 1, currentSquare, layerOfPoint);
                        break;
                    case 3:
                        CreatePoint(-1, 0, currentSquare, layerOfPoint);
                        break;
                    case 4:
                        //middle is never used
                        break;
                    case 5:
                        CreatePoint(1, 0, currentSquare, layerOfPoint);
                        break;
                    case 6:
                        CreatePoint(-1, -1, currentSquare, layerOfPoint);
                        break;
                    case 7:
                        CreatePoint(0, -1, currentSquare, layerOfPoint);
                        break;
                    case 8:
                        CreatePoint(1,-1,currentSquare,layerOfPoint);
                        break;
                }
            }

            //create line and create additional lines if more than one layer

            for (int currentLine = 0; currentLine < numberOfLinesPerSquare; currentLine++)
            {
                switch (currentLine)
                {
                    case 0:
                        CreateLine(-1f, 2, currentSquare, layerOfLine, lineHorizontalScale,1,0);
                        break;
                    case 1:
                        CreateLine(1f, 2, currentSquare, layerOfLine, lineHorizontalScale,1,0);
                        break;
                    case 2:
                        //rotate
                        CreateLine(-2, 1, currentSquare, layerOfLine, lineVerticalScale,0,1);
                        break;
                    case 3:
                        //rotate
                        CreateLine(2, 1, currentSquare, layerOfLine, lineVerticalScale,0,1);
                        break;
                    case 4:
                        //middle is never used
                        CreateLine(-2, -1, currentSquare, layerOfLine, lineVerticalScale,0,1);
                        break;
                    case 5:
                        CreateLine(2, -1, currentSquare, layerOfLine, lineVerticalScale,0,1);
                        break;
                    case 6:
                        CreateLine(-1, -2, currentSquare, layerOfLine, lineHorizontalScale,1,0);
                        break;
                    case 7:
                        CreateLine(1, -2, currentSquare, layerOfLine, lineHorizontalScale,1,0);
                        break;


                }
            }

            if ((currentSquare + 1) < numberOfSquares)
            {
                for(int currentConnection = 0; currentConnection < numberOfConnectionsBetweenSquares; currentConnection++)
                {
                    switch (currentConnection)
                    {
                        case 0:
                            CreateLine(0, 3, currentSquare, layerOfLine, lineVerticalScale,0,0,true,0,1);
                            break;
                        case 1:
                            CreateLine(-3, 0, currentSquare, layerOfLine, lineHorizontalScale, 0, 0, true,-1,0);
                            break;
                        case 2:
                            CreateLine(3, 0, currentSquare, layerOfLine, lineHorizontalScale, 0, 0, true,1,0);
                            break;
                        case 3:
                            CreateLine(0, -3, currentSquare, layerOfLine, lineVerticalScale, 0, 0, true,0,-1);
                            break;

                    }
                }
            }
        }


    }

    private void CreatePoint(int multiplierX, int multiplierY, int currentSquare, float layer)
    {
        positionOfGO = new Vector3((distanceLeftRight * (currentSquare + 1)) * multiplierX, (distanceTopBottom * (currentSquare + 1)) * multiplierY, layer);
        pointGO = Instantiate(point, positionOfGO, Quaternion.identity);
        pointGO.transform.parent = pointHolderGO.transform;
    }

    private void CreateLine(float multiplierX, float multiplierY, int currentSquare, float layer, Vector2 scale,
        float scaleXMultiplier = 0, float scaleYMultiplier = 0, bool connection = false, float leftRight = 0, float upDOwn = 0)
    {
        if (connection)
        {
            positionOfGO = new Vector3(((distanceLeftRightLine * multiplierX) +  (distanceBetweenSquares * (currentSquare) * leftRight)),
                ((distanceTopBotLine * multiplierY) +  (distanceBetweenSquares * (currentSquare) * upDOwn)),
                layer);
        }
        else
        {
            positionOfGO = new Vector3(((distanceLeftRightLine * multiplierX) * (currentSquare + 1)),
                ((distanceTopBotLine * multiplierY) * (currentSquare + 1)),
                layer);
        }
        pointGO = Instantiate(line, positionOfGO, Quaternion.identity);
        pointGO.transform.parent = lineHolderGO.transform;
        scale = new Vector2(scale.x + (scaleXMultiplier * distanceBetweenSquares * currentSquare), scale.y + (scaleYMultiplier * distanceBetweenSquares * currentSquare));
        pointGO.transform.localScale = scale;
    }

    public void DeleteGameField()
    {
        while(pointHolderGO.transform.childCount != 0)
        {
            Destroy(pointHolderGO.transform.GetChild(0));
        }

        while(lineHolderGO.transform.childCount != 0)
        {
            Destroy(lineHolderGO.transform.GetChild(0));
        }

    }
}
