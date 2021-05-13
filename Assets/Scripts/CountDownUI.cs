using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : SingletonMonoBehaviour<CountDownUI>
{
    Text text;


    void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        
    }
    public void StartCountDown(int time) {
        StartCoroutine(CountDown(time));
    }
    IEnumerator CountDown(int time) {
        text.gameObject.SetActive(true);
       
        for (int i = 0; i < time; i++) {
            text.text = (time - i).ToString();
            yield return new WaitForSeconds(1);
        }
        text.gameObject.SetActive(false);
    }
}
