using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter");

        SceneManager.LoadScene(2);

    //    if (other.GetComponent<PlayerMovementScript>())
    //    {
    //        Checkpoint.ResetSave();
    //        Checkpoint.SetSaveScene(TargetLevel);
    //        Checkpoint.loadScene();
    //    }

    }

}
