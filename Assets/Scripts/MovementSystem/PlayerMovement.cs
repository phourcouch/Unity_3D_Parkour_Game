using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 Velocity;
    CharacterController controller;
    CameraBobber CameraBob;
    public Animator playerAnims;
    bool bob = true;
    public float ang;
    public bool Grounded;
    public bool vaulted; // Has the player already vaulted since leaving the ground? 
    private bool canDoubleJump;
    private bool wallSliding;
    private bool canWallSlide;
    private float wallSlideTimer;
    private float cameraHeight;
    private float hitboxHeight;

    public float WallEjectForce;
    public float MaxSpeed;
    public float Deceleration;
    public float Gravity;
    public float Acceleration;
    public float JumpForce;
    public float midairMovementCap;
    public float midairAcceleration;
    public float maxWallSlideTime;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        CameraBob = gameObject.GetComponentInChildren<CameraBobber>();
        if (!gameObject.GetComponentInChildren<CameraBobber>())
        {
            bob = false;
        }
        
        hitboxHeight = controller.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraHeight == 0)
        {
            cameraHeight = CameraBob.bob.DefaultOffset.y;
        }
        
        bool g = Grounded;
        Grounded = CheckIsGrounded();

        float x = Input.GetAxis("Vertical");

        playerAnims.speed = 1 + x;
        if (x > 0)
        {
            playerAnims.SetBool("Moving", true);

        }
        else if (x < 0)
        {
            playerAnims.SetBool("Moving", true);
        }
        else
        {
            playerAnims.SetBool("Moving", false);
        }

        if (Grounded || controller.isGrounded) //When the player is grounded
        {
            canWallSlide = true;
            canDoubleJump = true;
            vaulted = false;

            if (!g) //If the player just landed
                hitGround();

            Velocity.y = 0;
            wallSlideTimer = 0;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Slide();
            }
            else
            {
                if (cameraHeight != 0)
                {
                    if (CameraBob.bob.DefaultOffset.y != cameraHeight)
                    {
                        CameraBob.bob.DefaultOffset.y = cameraHeight;
                        controller.height = hitboxHeight;
                        transform.position += new Vector3(0, controller.height / 2, 0);
                    }
                    
                    DoMovement(Acceleration, MaxSpeed, 1);
                }
                
            }
            
            Jump();

            if (bob && GetSpeed() >= 0.01f) // Camera animation stuff
            {
                CameraBob.WalkBob(GetSpeed());
            }
            else if (GetSpeed() < 0.01f)
            {
                CameraBob.StopWalk();
            }

        }
        else //When the player is midair
        {
            if (Velocity.y > -5f)
                checkVault();
            if (bob)
            {
                CameraBob.StopWalk();
            }
            CheckSidesOutput sides = checkSides();






            WallSlide(sides);

            

            if (wallSliding)
            {
                doGravity(Gravity*0.25f); //Slows gravity if the player is wallsliding
                
            }
            else
            {
                doGravity(Gravity);
                CameraBob.CameraRoll(false, 0);
            }

            DoMovement(midairAcceleration, midairMovementCap, 0.05f);
            if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                CameraBob.doubleJump();
                vaulted = false;
                canDoubleJump = false;
            }

            if (wallSliding && Input.GetKeyDown(KeyCode.Space)) //Ejects the player from the wall
            {
                Velocity = new Vector3();
                Velocity += HorizontalDirectionalForce(1, 0, sides.getAngle(), WallEjectForce/2f); 
                Velocity += HorizontalDirectionalForce(1, 0, ang, WallEjectForce / 2f);
                Velocity += new Vector3(0, JumpForce+0.1f, 0);

                print("Eject");
                wallSliding = false;
                canWallSlide = false;
                
                wallSlideTimer = 100000; // stupid but its the easiest way to make it so that you cant wallslide again. No point making a whole new variable and stuff
                canDoubleJump = true;
                CameraBob.CameraRoll(false, 0);
                //Debug.DrawLine(transform.position, transform.position + HorizontalDirectionalForce(1, 0, sides.getAngle() + 90, 10f), Color.red, 30f);
            }
        }

        

        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (Velocity.y > 0)
            {
                Velocity.y = 0;
            }
        }

        

        controller.Move(Velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((controller.collisionFlags & CollisionFlags.Above) == 0)
        {
            Vector3 diff = hit.normal * Vector3.Dot(Velocity, hit.normal);
            Velocity -= new Vector3(diff.x, 0, diff.z);
        }
    }

    public void Slide ()
    {
        CameraBob.bob.DefaultOffset.y = cameraHeight - hitboxHeight/4;
        controller.height = hitboxHeight / 2;
    }

    public void DoMovement(float AccelerationMultiplier, float cap, float dm)
    {
        

        Decelerate(dm);

        Vector3 SpeedAng = HorizontalDirectionalForce(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), ang, AccelerationMultiplier); // Gets the vector for the force to apply based on the movement (x for forward/backward, y for left/right, rotation for the angle, and multiplier for the force)

        float x = Mathf.Abs(SpeedAng.x) + Mathf.Abs(Velocity.x);
        float z = Mathf.Abs(SpeedAng.z) + Mathf.Abs(Velocity.z);

        float d = Mathf.Sqrt((x * x) + (z * z)); //Gets the total speed that the movement will be after the force is applied
        if (d > cap) // If that speed is greater than max speed, then make it so it's not (This is done so that its still possible to go over the max speed (like if theres an explosion or something that launches the player) but the player cannot walk faster than the max speed)
        {
            SpeedAng = new Vector3(SpeedAng.x * (cap / d), 0, SpeedAng.z * (cap / d));
            Velocity += SpeedAng;
        }
        else
        {
            Velocity += new Vector3(SpeedAng.x, 0, SpeedAng.z); //If not just apply normally 
        }

        

    }

    public float GetSpeed ()
    {
        float d = Mathf.Sqrt((Velocity.x * Velocity.x) + (Velocity.z * Velocity.z));

        return (d);
    }

    void Jump ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Grounded)
            {
                transform.position += new Vector3(0, 1f, 0);
            }
            Velocity.y = JumpForce;
            
        }
    }

    Vector3 HorizontalDirectionalForce(float x, float y, float rotation, float forcemultiplier)
    {
        float speed;
        speed = (Mathf.Abs(x) + Mathf.Abs(y)); //Turns the x and y into a speed value by averaging it and then clipping it off at 1
        if (speed > 1)
            speed = 1;
        speed *= forcemultiplier; // Multiplies the speed value by the speed multiplier

        Vector3 SpeedAng = new Vector3(0, 0, 0);

        if (Mathf.Sqrt(x * x + y * y) != 0) //Does not work if the speed is zero, also this saves computation 
        {
            int ca = 1;
            if (x < 0)
                ca = -1;
            if (x > 0)
                ca = 1;

            SpeedAng = new Vector3((1 * Mathf.Abs(speed) * Time.deltaTime) * (ca * Mathf.Sin((rotation + (Mathf.Atan(y / x) * Mathf.Rad2Deg)) * Mathf.Deg2Rad)), 0,
            (1 * Mathf.Abs(speed) * Time.deltaTime) * (ca * Mathf.Cos((rotation + (Mathf.Atan(y / x) * Mathf.Rad2Deg)) * Mathf.Deg2Rad))); // Converts xSpeed and ySpeed into an angle and then adds that angle to the rotation

        }
        return (SpeedAng);
    }

    void Decelerate (float m)
    {
        Velocity.x *= 1 - (Deceleration * Time.deltaTime * m);
        Velocity.z *= 1 - (Deceleration * Time.deltaTime * m);
    }

    void hitGround ()
    {
        if (!bob)
            return;
        else
        {
            CameraBob.hitGround(Velocity.y);
        }
        
    }

    void doGravity (float Force)
    {

        addVerticalForce(-Force * Time.deltaTime);
    }

    public void addVerticalForce(float force)
    {
        Velocity.y += force;
    }

    public bool CheckIsGrounded()
    {
        RaycastHit hit;

        float distance = 0.025f;
        float radius = controller.radius - 0.1f;

        Vector3 rayPos1 = new Vector3(transform.position.x + radius, transform.position.y, transform.position.z);
        Vector3 rayPos2 = new Vector3(transform.position.x - radius, transform.position.y, transform.position.z);
        Vector3 rayPos3 = new Vector3(transform.position.x, transform.position.y, transform.position.z + radius);
        Vector3 rayPos4 = new Vector3(transform.position.x, transform.position.y, transform.position.z - radius);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, controller.height / 2 + distance) || // Casting 5 rays down is a really costly and very poor way to check if grounded. In the future this is going to have to be reworked.
            Physics.Raycast(rayPos1, transform.TransformDirection(Vector3.down), out hit, controller.height / 2 + distance) ||
            Physics.Raycast(rayPos2, transform.TransformDirection(Vector3.down), out hit, controller.height / 2 + distance) || // I feel like there's a better way to do this, however I have no clue what that is
            Physics.Raycast(rayPos3, transform.TransformDirection(Vector3.down), out hit, controller.height / 2 + distance) ||
            Physics.Raycast(rayPos4, transform.TransformDirection(Vector3.down), out hit, controller.height / 2 + distance))
        {
            
            if (Grounded == false)
            {
                hitGround();
            }

            return true;
        }
        else
        {
            

            return false;
        }
    }

    public void checkVault()
    {
        if (vaulted)
        {
            return;
        }
        RaycastHit hit;
        Vector3 ray1Pos = new Vector3(transform.position.x, transform.position.y - 0.00f, transform.position.z);
        Vector3 ray2Pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        bool a = false;
        bool b = false;

        if (Physics.Raycast(ray1Pos, transform.TransformDirection(Vector3.forward), out hit, 0.75f))
        {

            a = true;
        }
        if (Physics.Raycast(ray2Pos, transform.TransformDirection(Vector3.forward), out hit, 0.75f))
        {
            b = true;

        }
        if (a && !b)
        {
            Velocity.y = 0;


            Velocity = HorizontalDirectionalForce(1, 0, ang, 15);
            addVerticalForce(7f);
            vaulted = true;
            print("vault");
            canDoubleJump = true; //If the player vaults, they get another double jump
        }
    }

    public CheckSidesOutput checkSides ()
    {
        RaycastHit hit;

        float distance = controller.radius+0.2f;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance)) //Casts four rays out in left, right, forward , and back directions and returns the angle of the surface as well as if any hit
        {                                                                                                          //In retrospect, I could've just done a single if statement for this, but I'll probably refactor later
            return (onRaycastTrue(hit));
            
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit, distance))
        {
            return (onRaycastTrue(hit));
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, distance))
        {
            return (onRaycastTrue(hit));
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, distance))
        {
            return (onRaycastTrue(hit));
        }
        else if (Physics.Raycast(transform.position, new Vector3(0, 45, 0), out hit, distance))
        {
            return (onRaycastTrue(hit));
        }
        else if (Physics.Raycast(transform.position, new Vector3(0, 135, 0), out hit, distance))
        {
            return (onRaycastTrue(hit));
        }
        else if (Physics.Raycast(transform.position, new Vector3(0, 225, 0), out hit, distance))
        {
            return (onRaycastTrue(hit));
        }
        else if (Physics.Raycast(transform.position, new Vector3(0, 315, 0), out hit, distance))
        {
            return (onRaycastTrue(hit));
        }
        else
        {
            //print("false");
            return (new CheckSidesOutput(0, false, new Vector3()));
        }

    }

    private CheckSidesOutput onRaycastTrue (RaycastHit hit)
    {
        float a = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg;


        //print(a + ", " + "true");
        return (new CheckSidesOutput(a, true, hit.normal));
    }

    void WallSlide (CheckSidesOutput o)
    {
        if (wallSlideTimer > maxWallSlideTime && wallSliding)
        {
            wallSliding = false;
            canDoubleJump = true;
            CameraBob.CameraRoll(false, 0);
            return;

        }
        if (!canWallSlide)
        {
            wallSliding = false;
            return;


        }
        if (o.getOutput() && Velocity.y > -2 && Velocity.y < 2)
        {
            if (!wallSliding)
            {
                if (Velocity.y < 0.5f)
                {
                    Velocity.y = 0.5f;
                }
                
                wallSliding = true;

                float a;
                
                if (Mathf.Sin(ang%360 - o.getAngle()) < 0)
                {
                    a = -4f;
                }
                else
                {
                    a = 4f;
                }

                Velocity += HorizontalDirectionalForce(1, 0, o.getAngle() - 180, 200f);

                CameraBob.CameraRoll(true, a);
                //Velocity.x = 0;
                //Velocity.z = 0;
                
            }
            else
            {
                float a;

                if (Mathf.Sin((ang % 360 - o.getAngle()) * Mathf.Deg2Rad) < 0)
                {
                    a = -10f;
                }
                else
                {
                    a = 10f;
                }
                CameraBob.CameraRoll(true, a);
            }
            wallSlideTimer += Time.deltaTime;
            canDoubleJump = false;
        }
        else if (wallSliding)
        {
            canDoubleJump = true;
            wallSliding = false;
            canWallSlide = false;
            CameraBob.CameraRoll(false, 0);
            return;
            
        }
    }

}

public class CheckSidesOutput
{
    float ang;
    bool didOutput;
    Vector3 normal;

    public CheckSidesOutput(float angle, bool o, Vector3 normal)
    {
        ang = angle;
        didOutput = o;
    }

    public float getAngle ()
    {
        return (ang);
    }

    public bool getOutput ()
    {
        return (didOutput);
    }

    public Vector3 getNormal ()
    {
        return (normal);
    }
}