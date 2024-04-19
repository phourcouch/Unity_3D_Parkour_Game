using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBox : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.tag == "Robot")
        {
            other.GetComponent<Animator>().SetTrigger("Sleep");
        }
    }
}
