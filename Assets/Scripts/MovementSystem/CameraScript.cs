using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraScript : MonoBehaviour
{
    public Transform parent;
    float xAng;
    float yAng;
    float time;
    public float sensitivity = 1f;
    float angOld;
    public float cameraHeight;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        xAng += Input.GetAxis("Mouse X") * sensitivity;
        yAng += Input.GetAxis("Mouse Y") * sensitivity;


        yAng = Mathf.Clamp(yAng, -75f, 55f);
        transform.localEulerAngles = new Vector3(-yAng, 0, 0);
        parent.GetComponent<PlayerMovement>().ang = xAng;
        parent.eulerAngles = new Vector3(0, xAng, 0);
    }
}
