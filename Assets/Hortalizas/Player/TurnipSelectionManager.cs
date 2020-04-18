using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipSelectionManager : MonoBehaviour
{
    List<GameObject> selectedTurnips;
    Camera main;

    //The start and end coordinates of the square we are making
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    float delay = 0.1f;
    bool hasCreatedSquare;
    //The selection squares 4 corner positions
    Vector3 TL, TR, BL, BR;

    public RectTransform selectionSquareTrans;


    void Start()
    {
        selectedTurnips = new List<GameObject>();
        main = Camera.main;
    }


    float clickTime = 0f;
    // Update is called once per frame
    void Update()
    {
        //Are we clicking with left mouse or holding down left mouse
        bool isClicking = false;
        bool isHoldingDown = false;

        //GetFirstDown
        if (Input.GetMouseButtonDown(0))
        {
            clickTime = Time.time;

            squareStartPos = main.ScreenToWorldPoint(Input.mousePosition);

            UnselectAll();
        }

        //Holding down the mouse button
        if (Input.GetMouseButton(0))
        {
            if (Time.time - clickTime > delay)
            {
                isHoldingDown = true;
            }
        }

        //Releasing the button
        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - clickTime <= delay)
            {
               RaycastHit2D hit = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Turnip"))
                    {
                        SelectTurnip(hit.collider.gameObject);
                        return;
                    }
                }
            }

            if (hasCreatedSquare)
            {
                hasCreatedSquare = false;

                //Deactivate the square selection image
                selectionSquareTrans.gameObject.SetActive(false);

                //Clear the list with selected unit
                UnselectAll();

                Vector3[] corners = new Vector3[4];
                selectionSquareTrans.GetWorldCorners(corners);

                Vector2 upCorner = main.ScreenToWorldPoint(corners[1]);
                Vector2 downCorner = main.ScreenToWorldPoint(corners[3]);
               
                Vector2 size = downCorner - upCorner;
                size.x = Mathf.Abs(size.x);
                size.y = Mathf.Abs(size.y);

                Vector2 origin = Vector2.zero;
                origin = main.ScreenToWorldPoint(selectionSquareTrans.transform.position);

                RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, 0, Vector2.zero);

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.CompareTag("Turnip"))
                    {
                        SelectTurnip(hits[i].collider.gameObject);
                    }
                }

            }
        }


        //Drag the mouse to select all units within the square
        if (isHoldingDown)
        {
            //Activate the square selection image
            if (!selectionSquareTrans.gameObject.activeInHierarchy)
            {
                selectionSquareTrans.gameObject.SetActive(true);
            }

            //Get the latest coordinate of the square
            squareEndPos = Input.mousePosition;

            //Display the selection with a GUI image
            DisplaySquare();
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == null && selectedTurnips.Count > 0)
            {
                SetDestinationForSelected(mousePos);
            }
        }
    }

    void UnselectAll()
    {
        for (int i = 0; i < selectedTurnips.Count; i++)
        {
            selectedTurnips[i].transform.GetChild(1).gameObject.SetActive(false);
        }
        selectedTurnips.Clear();
    }

    void SelectTurnip(GameObject turnip)
    {
        selectedTurnips.Add(turnip);
        turnip.transform.GetChild(1).gameObject.SetActive(true);
    }

    void SetDestinationForSelected(Vector3 mousePos)
    {
        for (int i = 0; i < selectedTurnips.Count; i++)
        {
            selectedTurnips[i].GetComponent<TurnipBehaviour>().SetTargetPosition(mousePos);
        }
    }

    void DisplaySquare()
    {
        //The start position of the square is in 3d space, or the first coordinate will move
        //as we move the camera which is not what we want
        Vector3 squareStartScreen = Camera.main.WorldToScreenPoint(squareStartPos);

        squareStartScreen.z = 0f;

        //Get the middle position of the square
        Vector3 middle = (squareStartScreen + squareEndPos) / 2f;

        //Set the middle position of the GUI square
        selectionSquareTrans.position = middle;

        //Change the size of the square
        float sizeX = Mathf.Abs(squareStartScreen.x - squareEndPos.x);
        float sizeY = Mathf.Abs(squareStartScreen.y - squareEndPos.y);

        //Set the size of the square
        selectionSquareTrans.sizeDelta = new Vector2(sizeX, sizeY);

        //The problem is that the corners in the 2d square is not the same as in 3d space
        //To get corners, we have to fire a ray from the screen
        //We have 2 of the corner positions, but we don't know which,  
        //so we can figure it out or fire 4 raycasts
        TL = new Vector3(middle.x - sizeX / 2f, middle.y + sizeY / 2f, 0f);
        TR = new Vector3(middle.x + sizeX / 2f, middle.y + sizeY / 2f, 0f);
        BL = new Vector3(middle.x - sizeX / 2f, middle.y - sizeY / 2f, 0f);
        BR = new Vector3(middle.x + sizeX / 2f, middle.y - sizeY / 2f, 0f);

        //From screen to world
        RaycastHit hit;
        int i = 0;
        //Fire ray from camera
        if (Physics.Raycast(main.ScreenPointToRay(TL), out hit, 200f, 1 << 8))
        {
            TL = hit.point;
            i++;
        }
        if (Physics.Raycast(main.ScreenPointToRay(TR), out hit, 200f, 1 << 8))
        {
            TR = hit.point;
            i++;
        }
        if (Physics.Raycast(main.ScreenPointToRay(BL), out hit, 200f, 1 << 8))
        {
            BL = hit.point;
            i++;
        }
        if (Physics.Raycast(main.ScreenPointToRay(BR), out hit, 200f, 1 << 8))
        {
            BR = hit.point;
            i++;
        }

        hasCreatedSquare = true;
    }

}
