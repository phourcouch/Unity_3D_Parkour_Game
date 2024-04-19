using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour
{
    public Animator anim;
    public Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    public float meleeRange = 4f;
    public float rotationSpeed = 10f;
    public GameObject bullet;
    public Transform shootLocation;
    Vector3 velocity;
    float ztrans;
    Transform newBullet;
    private float timer = 0f;
    private float kbtimer = 3f;



    // Use this for initialization
    void Start()
    {
     
        anim = GetComponent<Animator>();
        CharacterController controller = GetComponent<CharacterController>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f) timer = 3f;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                attack();
               
            }
           
        }
                
        if(newBullet != null)
        {
            float bulletDist = Vector3.Distance(target.position, newBullet.transform.position);
            if (bulletDist > 0.8 )
            {
                float Speed = 5f * Time.deltaTime;

                newBullet.transform.Translate(Vector3.forward * Time.deltaTime * 5f, transform);
              
            }
            else if (bulletDist < 0.8)
            {
                Destroy(newBullet.gameObject);
            }
        }

        float dist = Vector3.Distance(target.position, transform.position);
     


  
       
        

        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
          

            RotateTowards(target);
            anim.SetBool("Run", false);

        }
      
     
    

        if (agent.velocity.x > 0)
        {

           
            anim.SetBool("Run", true);

        }
        else if (agent.velocity.x < 0)
        {
            anim.SetBool("Run", true);
         
        }
        else if (agent.velocity.y > 0)
        {
            anim.SetBool("Run", true);
        

        }
        else if (agent.velocity.y < 0)
        {
            anim.SetBool("Run", true);
          
        }
        else
        {
            anim.SetBool("Run", false);
        }






    }
    
     void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    
    void attack()
    {
      
        anim.SetBool("Attack", true);
        agent.velocity = Vector3.zero;
        Invoke("shoot", 0.2f);
      Invoke("attackfalse", 0.5f);//this will happen after 2 seconds
        timer = 0f;
    }
    void attackfalse()
    {
     
        anim.SetBool("Attack", false);
    }
    void shoot()
    {
        newBullet = Instantiate(bullet.transform, shootLocation.position, bullet.transform.rotation);
        Destroy(newBullet.gameObject, 4);

    }
  
} 