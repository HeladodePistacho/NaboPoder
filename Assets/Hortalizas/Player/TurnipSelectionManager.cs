using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipSelectionManager : MonoBehaviour
{
    List<GameObject> selectedTurnips;
    public Camera main;
    public GameObject allTurnips;
    public LayerMask layerForPoint;
    public LayerMask layerForSquare;

    //The start and end coordinates of the square we are making
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    float delay = 0.17f;
    bool hasCreatedSquare;
    //The selection squares 4 corner positions
    Vector3 TL, TR, BL, BR;

    public RectTransform selectionSquareTrans;
    Vector3 selectedPosition = Vector3.zero;

    Transform hover;
    PlayerStats playerStats;
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
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

        Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0, layerForPoint);
        if (hit.collider != null)
        {
            if (hover != null)
            {
                hover.GetChild(0).gameObject?.SetActive(false);
                hover = null;
            }

            GameObject selectionMark = hit.transform.GetChild(0).gameObject;

            TurnipBehaviour tb = hit.transform.GetComponent<TurnipBehaviour>();
            if (tb == null || tb.selected == false)
            {
                selectionMark.SetActive(true);
                hover = hit.transform;
                Debug.Log("Hover");
            }


        }
        else if (hover != null)
        {
            Debug.Log("StopHover");
            hover.GetChild(0).gameObject?.SetActive(false);
            hover = null;

        }

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
                if (hover != null)
                {
                    if (hover.CompareTag("Turnip"))
                    {
                        SelectTurnip(hover.gameObject);

                        return;
                    }

                    if (hover.CompareTag("PlantableTile"))
                    {
                        ManageTile(hover.gameObject.GetComponent<PlantableTileController>());
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

                RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, 0, Vector2.zero, 0, layerForSquare);

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
            return;
        }

        if (Input.GetMouseButton(1))
        {
            if (hover && hover.CompareTag("Enemy"))
            {
                SetDestinationForSelected(hit.collider.transform);
                return;
            }

            mousePos = main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == null)
            {
                if (selectedTurnips.Count > 0)
                {
                    SetDestinationForSelected(mousePos);
                    return;
                }
            }

            return;
        }

        if (Input.GetMouseButtonDown(2))
            ReturnToPlayer();
    }

    void UnselectAll()
    {
        for (int i = 0; i < selectedTurnips.Count; i++)
        {
            if (selectedTurnips[i] == null)
                continue;
            selectedTurnips[i].transform.GetChild(0).gameObject.SetActive(false);
            selectedTurnips[i].GetComponent<TurnipBehaviour>().selected = false;
        }
        selectedTurnips.Clear();
    }

    void SelectTurnip(GameObject turnip)
    {
        selectedTurnips.Add(turnip);
        turnip.transform.GetChild(0).gameObject.SetActive(true);
        turnip.GetComponent<TurnipBehaviour>().selected = true;
        hover = null;
    }

    void SetDestinationForSelected(Vector3 mousePos)
    {
        for (int i = 0; i < selectedTurnips.Count; i++)
        {
            if (selectedTurnips[i] == null)
                continue;
            selectedTurnips[i].GetComponent<TurnipBehaviour>().SetTargetPosition(mousePos);
        }
    }
    void SetDestinationForSelected(Transform target)
    {
        for (int i = 0; i < selectedTurnips.Count; i++)
        {
            if (selectedTurnips[i] == null)
                continue;
            selectedTurnips[i].GetComponent<TurnipBehaviour>().SetTarget(target);
        }
    }

    void ReturnToPlayer()
    {
        if (selectedTurnips.Count == 0)
        {
            for (int i = 0; i < allTurnips.transform.childCount; i++)
            {
                TurnipBehaviour tb = allTurnips.transform.GetChild(i).GetComponent<TurnipBehaviour>();
                if (tb.turnipState != TurnipState.FOLLOWING_PLAYER && tb.turnipState != TurnipState.WAITING_FOR_PLAYER)
                {
                    tb.SetState_FollowPlayer();
                }
            }
        }

        else for (int i = 0; i < selectedTurnips.Count; i++)
            {
                if (selectedTurnips[i] == null)
                    continue;
                TurnipBehaviour tb = selectedTurnips[i].GetComponent<TurnipBehaviour>();
                if (tb.turnipState != TurnipState.FOLLOWING_PLAYER && tb.turnipState != TurnipState.WAITING_FOR_PLAYER)
                {
                    tb.SetState_FollowPlayer();
                }
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


    public AudioSource audioSource;
    public AudioClip noSeedsClip;
    public AudioClip plantClip;
    public AudioClip collectClip;
    void ManageTile(PlantableTileController tileController)
    {
        if (tileController.tileState == TileState.PLANTABLE)
        {
            if (playerStats.GetSeeds() > 0)
            {
                tileController.PlantTile();
                playerStats.AddSeeds(-1);
                audioSource.PlayOneShot(plantClip, 0.1f);
            }
            else
            {
                audioSource.PlayOneShot(noSeedsClip, 0.1f);
            }

        }
        else if (tileController.tileState == TileState.READY_TO_COLLECT)
        {
            audioSource.PlayOneShot(collectClip, 0.1f);
            tileController.CollectTile();
        }

        hover.GetChild(0).gameObject?.SetActive(false);
        hover = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(selectedPosition, 0.3f);
    }

}
