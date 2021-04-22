using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text CountText;
    [SerializeField] float CountTime = 4.0f;
    int count = 1;

    // Start is called before the first frame update
    void Start()
    {

        var a = FindObjectsOfType<Component>();
        foreach (var c in a)
        {
            var timeStopable = c as TimeStopable;

            if (timeStopable != null)
            {
                timeStopable.TimeStopped();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (count > 0)
        {
            count = (int)CountTime;
            CountText.text = count.ToString();
            CountTime -= 0.01f;
            Time.timeScale = 0;
        }
        else if (count <= 0)
        {
            Time.timeScale = 1;
            CountText.text = "";

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
    }

}
