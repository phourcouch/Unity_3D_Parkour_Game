using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingController : MonoBehaviour
{
    public List<Oscillator> AllBounces = new List<Oscillator>();  
    public Vector3 DefaultOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;


        Vector3 offset = new Vector3(0, 0, 0);
        
        foreach (Oscillator oscI in AllBounces.ToArray())
        {
            oscI.time += Time.deltaTime;

            offset += oscI.GetOutput();
            //print(offset);
            i++;
        }
        //print(offset);

        transform.localPosition = DefaultOffset;
        transform.localPosition += offset;
    }

    public void AddBounce (Oscillator osc)
    {
        AllBounces.Add(osc);
        osc.parent = this;
    }

}
