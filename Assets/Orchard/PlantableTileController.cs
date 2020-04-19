using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
    PLANTABLE,
    PLANTED,
    READY_TO_COLLECT
}
public class PlantableTileController : MonoBehaviour
{
    public Color plantableColor;
    public Color plantedColor;
    public Color readyColor;

   public TileState tileState = TileState.PLANTABLE;
    public float timeUntilReady = 2f;

    public GameObject turnipPrefab;
    public Transform allTurnips;

    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = plantableColor;
    }
    public void PlantTile()
    {
        tileState = TileState.PLANTED;
        spriteRenderer.color = plantedColor;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeUntilReady);

        tileState = TileState.READY_TO_COLLECT;
        spriteRenderer.color = readyColor;
    }

    public void CollectTile()
    {
        tileState = TileState.PLANTABLE;
        spriteRenderer.color = plantableColor;
        Instantiate<GameObject>(turnipPrefab, transform.position, Quaternion.identity, allTurnips);
    }
}
