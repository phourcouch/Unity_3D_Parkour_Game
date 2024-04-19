using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public GameObject Player;
    
    void Start()
    {
        Player.GetComponent<PlayerMovement>().enabled = false;
        Player.GetComponent<CharacterController>().enabled = false;
        Camera.main.GetComponent<BobbingController>().enabled = false;
        Camera.main.GetComponent<CameraBobber>().enabled = false;
        Camera.main.GetComponent<CameraScript>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        
    }
}
