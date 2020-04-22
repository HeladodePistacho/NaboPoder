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
     Animator anim;

   public TileState tileState = TileState.PLANTABLE;
    public float timeUntilReady = 2f;

    public GameObject turnipPrefab;
    public Transform allTurnips;

    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    public void PlantTile()
    {
        tileState = TileState.PLANTED;
        anim.SetInteger("TileState", 1);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeUntilReady);

        tileState = TileState.READY_TO_COLLECT;
        anim.SetInteger("TileState", 2);
    }

    public void CollectTile()
    {

        tileState = TileState.PLANTABLE;
        anim.SetInteger("TileState", 0);
        Instantiate<GameObject>(turnipPrefab, transform.position, Quaternion.identity, allTurnips);
    }
}
