using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRobotScript : MonoBehaviour
{
    public float torque = 500f;
    public float thrust = 1000f;
    public float rotationSpeed = 10f;
    private Rigidbody rb;
    public Transform player;
    private Vector3 position;
    private bool collision;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private float timer = 0f;
    public Animator anim;
    public GameObject bullet;
    public Transform shootLocation;

    Transform newBullet;
    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        position = transform.position;
        collision = false;
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
        if (newBullet != null)
        {
           
            float bulletDist = Vector3.Distance(player.position, newBullet.transform.position);
            if (bulletDist > 0.8)
            {
                var step = 5f * Time.deltaTime; // calculate distance to move
             //   newBullet.transform.position = Vector3.MoveTowards(newBullet.transform.position, player.position, step);
                 float Speed = 5f * Time.deltaTime;

                  newBullet.transform.Translate(Vector3.forward * Time.deltaTime * 5f, transform);

            }
            else if (bulletDist <= 0.8)
            {
                Destroy(newBullet.gameObject);
            }
        }

        Vector3 targetPosition = player.TransformPoint(new Vector3(-5, 0, 7));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);

        float dist = Vector3.Distance(player.position, transform.position);
     /*   if (dist <= 3f)
        {
            moveBack();
        }
        else
        {
            transform.Translate(Vector3.forward * 15f * Time.deltaTime);
        }
     */
        //  float dist = Vector3.Distance(player.position, transform.position);
        RotateTowards(player);

        Vector3 targetLocation = player.position - transform.position;
        float distance = targetLocation.magnitude;
     //   rb.AddRelativeForce(Vector3.forward * Mathf.Clamp((distance - 10) / 50, 0f, 1f) * thrust);
    }
    void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void bulletRotateTowards(Transform target)
    {
        Vector3 direction = (target.position - newBullet.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        newBullet.transform.rotation = Quaternion.Slerp(newBullet.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    void OnCollisionEnter(Collision other)
    {
        //    print("hit");
        collision = true;
        rb.AddRelativeForce(Vector3.up * thrust);
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        collision = false;
        rb.AddRelativeForce(Vector3.down * 30000);
    }
    void moveBack()
    {
        //    Vector3 dir = transform.position - player.position;
        transform.Translate(-Vector3.forward * 15f * Time.deltaTime);
    }
    void attack()
    {

        anim.SetBool("Attack", true);
      //  agent.velocity = Vector3.zero;
        Invoke("shoot", 0.2f);
        Invoke("attackfalse", 0.8f);//this will happen after 2 seconds
        timer = 0f;
    }
    void shoot()
    {
        Quaternion bulletRot = Quaternion.Euler(45, 0, 0);
        newBullet = Instantiate(bullet.transform, shootLocation.position, bulletRot);

      //  newBullet.transform.LookAt(player.transform.position);


        //     newBullet.transform.rotation = Quaternion.identity;
        //     bulletRotateTowards(player);
        Destroy(newBullet.gameObject, 5);

    }
    void attackfalse()
    {

        anim.SetBool("Attack", false);
    }
}
