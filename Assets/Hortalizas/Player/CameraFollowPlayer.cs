using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float smooth_speed;

    Vector3 new_pos;

    // Start is called before the first frame update
    void Start()
    {
        new_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //aki
        Vector3 desired_pos = player.transform.position;
        
        float smooth_x = Mathf.Lerp(transform.position.x, desired_pos.x, smooth_speed * Time.deltaTime);
        float smooth_y = Mathf.Lerp(transform.position.y, desired_pos.y, smooth_speed * Time.deltaTime);

        new_pos.x = speed * smooth_x;
        new_pos.y = speed * smooth_y;

        Debug.Log(new_pos);

        transform.position = new_pos;
    }
}
