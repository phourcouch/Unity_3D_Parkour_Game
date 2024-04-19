using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFollowScript : MonoBehaviour
{

    public Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;

    public float SpotLevel;

    static float moveSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; 

        if (Vector3.Distance(transform.position, target.position) < 50 && !Physics.Raycast(transform.position, target.position))
        {
            SpotLevel += Time.deltaTime*0.5f;
            if (SpotLevel > 3)
            {
                SpotLevel = 3;
            }
        }
        else {
            SpotLevel -= Time.deltaTime * 0.1f;
            if (SpotLevel < 0)
            {
                SpotLevel = 0;
            }
        }
        
        if (SpotLevel > 2)
        {
            if (Vector3.Distance(transform.position, target.position) > 5 && !Physics.Raycast(transform.position, target.position))
            {
                agent.SetDestination(target.position);
            }
            else
            {

            }
        }

        moveSpeed = 2;

        anim.SetFloat("Speed", moveSpeed);

    }



}
