using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float max_move_speed = 0.0f;
    public float movement_acceleration = 0.0f;
    public float friction = 0.0f;

    Vector3 current_movement_speed;

    int last_axis = 0;
    
    //Animations
    Animator anim;
    SpriteRenderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        current_movement_speed = new Vector3(0.0f, 0.0f, 0.0f);
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        int h_axis = (int)Input.GetAxisRaw("Horizontal");
        int v_axis = (int)Input.GetAxisRaw("Vertical");

        if (Mathf.Sign(current_movement_speed.x) != h_axis)
            current_movement_speed.x += (float)h_axis * friction * Time.deltaTime;

        if (Mathf.Sign(current_movement_speed.y) != v_axis)
            current_movement_speed.y += (float)v_axis * friction * Time.deltaTime;

        current_movement_speed.x += (float)h_axis * movement_acceleration * Time.deltaTime;
        current_movement_speed.y += (float)v_axis * movement_acceleration * Time.deltaTime;

        current_movement_speed.x = Mathf.Clamp(current_movement_speed.x, -max_move_speed, max_move_speed);
        current_movement_speed.y = Mathf.Clamp(current_movement_speed.y, -max_move_speed, max_move_speed);

        if (h_axis == 0)
        {
            //Decelerate
            current_movement_speed.x -= Mathf.Sign(current_movement_speed.x) * friction * Time.deltaTime;
            if (Mathf.Abs(current_movement_speed.x) <= 1)
            {
                current_movement_speed.x = 0.0f;
            }
        }

        if (v_axis == 0)
        {
            //Decelerate
            current_movement_speed.y -= Mathf.Sign(current_movement_speed.y) * friction * Time.deltaTime;
            if (Mathf.Abs(current_movement_speed.y) <= 1) current_movement_speed.y = 0.0f;
        }


        ManageAnimation();

        transform.Translate(current_movement_speed * Time.deltaTime);
    }
   
    void ManageAnimation()
    {
        if (current_movement_speed.x == 0.0f && current_movement_speed.y == 0)
            anim.SetBool("Moving", false);
        else
        {

            if (current_movement_speed.x > 0) renderer.flipX = true;
            else renderer.flipX = false;

            anim.SetBool("Moving", true);
        }
    }

}
