using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera player_camera;
    public Camera static_camera;

   public TurnipSelectionManager selectionManager;

    // Start is called before the first frame update
    void Start()
    {
        ActivatePlayerCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            ActivateStaticCamera();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ActivatePlayerCamera();
        }
    }

    void ActivateStaticCamera()
    {
        static_camera.enabled = true;
        static_camera.gameObject.GetComponent<AudioListener>().enabled = true;

        player_camera.enabled = false;
        player_camera.gameObject.GetComponent<AudioListener>().enabled = false;

        selectionManager.main = static_camera;
    }

    void ActivatePlayerCamera()
    {
        static_camera.enabled = false;
        static_camera.gameObject.GetComponent<AudioListener>().enabled = false;

        player_camera.enabled = true;
        player_camera.gameObject.GetComponent<AudioListener>().enabled = true;

        selectionManager.main = player_camera;

    }
}
