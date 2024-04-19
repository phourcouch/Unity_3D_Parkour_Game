using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobber : MonoBehaviour
{
    public BobbingController bob;
    Oscillator walk = new Oscillator(0, 0, new Vector3(), new Vector3());
    bool walking;
    public bool WalkBobEnabled = true;
    public bool HitFloorBob = true;
    private float t;

        // Start is called before the first frame update
    void Start()
    {
        bob = gameObject.GetComponent<BobbingController>();
        if (PlayerPrefs.GetInt("CameraBob") == 0)
        {
            WalkBobEnabled = false;
            HitFloorBob = false;
        }
        if (PlayerPrefs.GetInt("CameraBob") == 1)
        {
            WalkBobEnabled = true;
            HitFloorBob = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WalkBob (float Speed)
    {
        if (!WalkBobEnabled)
            return;
        if (!walking)
        {
            walk = new Oscillator(10f, 2f, new Vector3(-0.25f, 0, 0.75f), new Vector3(0.00f, 0.1f, 0.00f));
            bob.AddBounce(walk);
            walking = true;
        }
        else
        {
            walk.Amplitude = new Vector3(0f, Mathf.Lerp(walk.Amplitude.y, Speed * 0.01f, 10f), 0f);
        }
        
        
    }

    public void StopWalk ()
    {
        walk.FadeOut();
        walking = false;
    }

    public void hitGround (float Force)
    {
        if (!HitFloorBob)
        {
            return;
        }
        //Oscillator floorA = new Oscillator(20f, 1.25f, new Vector3(0, 0, 0), new Vector3(0, Force * 0.005f, 0));
        Oscillator floorB = new Oscillator(10f, 1.25f, new Vector3(0, 0, 0), new Vector3(0, Force * 0.02f, 0));
        //bob.AddBounce(floorA);
        bob.AddBounce(floorB);
        //floorA.FadeOut();
        floorB.FadeOut();
        print(Force);
    }

    public void doubleJump()
    {
        if (!HitFloorBob)
        {
            return;
        }
        Oscillator joe = new Oscillator(5f, 1.25f, new Vector3(0, 0, 0), new Vector3(0, 0.15f, 0));

        bob.AddBounce(joe);
        joe.FadeOut();

    }

    public void CameraRoll (bool stop, float ang)
    {
        if (stop)
        {
            t += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, ang), t*20);
            print(transform.localEulerAngles.z);
        }
        else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            t = 0;
        }
    }

}
