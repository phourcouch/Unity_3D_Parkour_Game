using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePlayerPrefsSetting : MonoBehaviour
{
    public string preference;       //Theres probably a significantly better approach to this but that doesn't matter because we dont need a super advanced main menu
    public bool enabledOrDisabled;
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        // Check the playerpref and if it is true call SetEnabled() and if it is false call SetDisabled()
        img = gameObject.GetComponent<Image>();
        int a = PlayerPrefs.GetInt(preference, -1);
        if (a == 0)
        {
            SetDisabled();
        }
        if (a == 1)
        {
            SetEnabled();
        }
        if (a == -1)
        {
            SetEnabled();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle ()
    {
        //Set the playerpref to the opposite of what it is and change it visually too with SetEnabled() and SetDisabled()
        int a = PlayerPrefs.GetInt(preference);
        if (a == 0)
        {
            SetEnabled();
        }
        if (a == 1)
        {
            SetDisabled();
        }
    }

    void SetEnabled ()
    {
        PlayerPrefs.SetInt(preference, 1);
        img.color = Color.white;
        // Turn the checkbox white
    }

    void SetDisabled()
    {
        PlayerPrefs.SetInt(preference, 0);
        img.color = Color.black;
        // Turn the checkbox black
    }
}
