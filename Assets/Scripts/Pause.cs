using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool GameIsPasue = false;
    public GameObject ingameMenu;
    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if(!GameIsPasue)
            {
                Pasue();
            }
            else
            {
                Resume();
            }
        }
    }
    public void Pasue()
    {
        Time.timeScale = 0;
        ingameMenu.SetActive(true);
        GameIsPasue = true;
        TimeStop();

    }

    public void Resume()
    {
        Time.timeScale = 1f;
        ingameMenu.SetActive(false);
        GameIsPasue = false;


        var a = FindObjectsOfType<Component>();
        foreach (var c in a)
        {
            var timeStopable = c as TimeStopable;

            if (timeStopable != null)
            {
                timeStopable.TimeReStarted();
            }
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }


    void TimeStop()
    {
        var components = new List<TimeStopable>();

        var a = FindObjectsOfType<Component>();
        foreach (var c in a)
        {
            var timeStopable = c as TimeStopable;

            if (timeStopable != null)
            {
                timeStopable.TimeStopped();
                components.Add(timeStopable);
            }
        }
    }

}
