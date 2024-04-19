using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickDetectors : MonoBehaviour
{
    public Camera cam;
    public GameObject Player;
    AudioSource LanternPickup;
    public Image Cursor;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray click = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(click, out hit, 5f))
            {
                switch (hit.collider.tag)
                    {
                        case "":
                            
                            break;
                    }
            }
        }
    }
}
