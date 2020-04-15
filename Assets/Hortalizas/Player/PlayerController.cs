using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float max_move_speed = 0.0f;
    public float movement_acceleration = 0.0f;
    public float friction = 0.0f;

    float current_movement_speed = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        HorizontalMovement();




    }

    void HorizontalMovement()
    {

        float h_axis = Input.GetAxisRaw("Horizontal");
        float v_axis = Input.GetAxisRaw("Vertical");

        current_movement_speed += h_axis * movement_acceleration * Time.deltaTime;

        current_movement_speed = Mathf.Clamp(current_movement_speed, -max_move_speed, max_move_speed);

        if(h_axis == 0)
        {
            //Decelerate
            current_movement_speed -= Mathf.Sign(current_movement_speed) * friction * Time.deltaTime;
            if (Mathf.Abs(current_movement_speed) < 0.5) current_movement_speed = 0.0f;
        }


        transform.Translate(current_movement_speed * Time.deltaTime, 0.0f, 0.0f);
    }
   
}
