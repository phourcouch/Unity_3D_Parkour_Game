using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator 
{
    public BobbingController parent;
    public Vector3 output;
    public Vector3 tOffset;
    public float time;
    public float frequency;
    public float tMult;
    public float cutoff;
    public Vector3 Amplitude;
    public float fTime;
    bool fadeOut;

    public Oscillator (float f, float tM, Vector3 tOf, Vector3 amp)
    {
        frequency = f;
        tMult = tM;
        tOffset = tOf;
        Amplitude = amp;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
    }

    public Vector3 GetOutput ()
    {
        output = new Vector3(0, 0, 0);
        if (fadeOut)
        {
            cutoff = 1 - (tMult * (time - fTime));
            if (cutoff <= 0)
            {

                if (parent != null)
                {
                    parent.AllBounces.Remove(this);
                }
                return (new Vector3(0, 0, 0));
            }
        }
        else
        {
            cutoff = 1;
        }
        
        output.x = Mathf.Sin((time + tOffset.x) * frequency) * Amplitude.x * cutoff;
        output.y = Mathf.Sin((time + tOffset.y) * frequency) * Amplitude.y * cutoff;
        output.z = Mathf.Sin((time + tOffset.z) * frequency) * Amplitude.z * cutoff;
        return (output);
    }

    public void FadeOut ()
    {
        if (!fadeOut)
        {
            fTime = time;
            fadeOut = true;
        }
        
    }

    public void ChangeFrequency (float Frequency)
    {
        frequency = Frequency;
    }

}
