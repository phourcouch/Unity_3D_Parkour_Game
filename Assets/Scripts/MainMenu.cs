using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    public GameObject Player;
    public GameObject Victor;
    public GameObject Sword;
    public Animator Canvas;
    public GameObject slider;
    public GameObject cam;
    public GameObject Menu;
    public PlayableDirector timeline1;
    public PlayableDirector timeline2;

    AudioSource intro;
    AudioSource ambient;
    AudioSource GenMusic;
    bool GameStart;

    void Awake()
    {
        intro = GameObject.Find("AudioManager").GetComponent<AudioManager>().Create("IntroMusic", cam);
        ambient = GameObject.Find("AudioManager").GetComponent<AudioManager>().Create("MenuAmbience", cam);
        GenMusic = GameObject.Find("AudioManager").GetComponent<AudioManager>().Create("AmbientMusic", cam);
    }

    void Start()
    {
        Player.GetComponent<PlayerMovement>().enabled = false;
        Player.GetComponent<CharacterController>().enabled = false;
        Victor.GetComponent<VictorsAttacks>().enabled = false;
        Camera.main.GetComponent<BobbingController>().enabled = false;
        Camera.main.GetComponent<CameraBobber>().enabled = false;
        Camera.main.GetComponent<CameraScript>().enabled = false;
        Cursor.visible = true;
        slider.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        ambient.Play();
    }
    void Update()
    {
        if (intro.isPlaying && ambient.volume > 0)
        {
            ambient.volume -= Time.deltaTime/3;
        }

        if (!intro.isPlaying && !GenMusic.isPlaying && GameStart)
        {
            GenMusic.Play();
        }
    }

    public void PressStart()
    {
        StartCoroutine(StartingIntro());
    }

    IEnumerator StartingIntro()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Canvas.SetTrigger("GameStarted");
        intro.Play();

        yield return new WaitForSeconds(1f);

        timeline1.Stop();
        Menu.SetActive(false);
        timeline2.Play();

        yield return new WaitForSeconds((float)timeline2.duration);

        timeline2.Stop();
        Player.GetComponent<PlayerMovement>().enabled = true;
        Player.GetComponent<CharacterController>().enabled = true;
        Victor.GetComponent<VictorsAttacks>().enabled = true;
        Camera.main.GetComponent<BobbingController>().enabled = true;
        Camera.main.GetComponent<CameraBobber>().enabled = true;
        Camera.main.GetComponent<CameraScript>().enabled = true;

        Sword.SetActive(false);
        GameStart = true;
        slider.SetActive(true);
    }
}
