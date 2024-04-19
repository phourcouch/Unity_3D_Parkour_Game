using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictorsAttacks : MonoBehaviour
{
    public GameObject Victor;
    public Slider slider;
    public GameObject sword;

    float actualHealth;
    float time;
    int attackNum;

    void Start()
    {
        slider.value = 100f;
        actualHealth = 1000f;
        time = 0.3f;
        attackNum = 0;
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && time <= 0)
        {
            sword.SetActive(true);
            attackNum = Random.Range(0, 2);
            switch (attackNum)
            {
                case 0:
                    Victor.GetComponent<Animator>().SetTrigger("Attack1");
                    break;
                case 1:
                    Victor.GetComponent<Animator>().SetTrigger("Attack2");
                    break;
            }
            time = 0.3f;
        }

        if (time <= 0.1f)
        {
            sword.SetActive(false);
        }
    }
}
