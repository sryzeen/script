using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
public class PauseMenu : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static bool menuScreen = false;
    public GameObject MenuUI;
    public GameObject PlayUI;
    //Animation showUP;

    /*void Awake()
    {
        showUP = GetComponent<Animation>();
    }*/
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuScreen)
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        Start();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MenuUI.SetActive(false);
        PlayUI.SetActive(true);
        Time.timeScale = 1f;
        menuScreen = false;
    }

    public void Pause()
    {
        //showUP.CrossFade("ShowUP");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MenuUI.SetActive(true);
        PlayUI.SetActive(false);
        Time.timeScale = 0f;
        menuScreen = true;
    }
}