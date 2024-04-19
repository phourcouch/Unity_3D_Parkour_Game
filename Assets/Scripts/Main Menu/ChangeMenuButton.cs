using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenuButton : MonoBehaviour
{
    public GameObject previousMenu;
    public GameObject nextMenu;
    //This is a really terrible, inflexible, and janky approach to switching menus, 
    //however there's really no point making a whole proper system for it 
    //because we don't need to do anything complex with the menus

    public void changeMenu ()
    {
        previousMenu.SetActive(false);
        nextMenu.SetActive(true);
    }
}
