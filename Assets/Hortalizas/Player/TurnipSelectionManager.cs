using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnipSelectionManager : MonoBehaviour
{
    List<GameObject> selectedTurnips;
    Camera main;

    void Start()
    {
        selectedTurnips = new List<GameObject>();
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Turnip"))
                {
                    SelectTurnip(hit.collider.gameObject);
                    return;
                }
            }

            UnselectAll();
        }

        if(Input.GetMouseButton(1))
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
            selectedTurnips.Clear();
        }
    }

    void SelectTurnip(GameObject turnip)
    {
        UnselectAll();
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
}
