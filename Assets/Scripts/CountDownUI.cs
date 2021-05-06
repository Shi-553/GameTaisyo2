using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : SingletonMonoBehaviour<CountDownUI>
{
    Text text;

    void Start()
    {
        text = transform.GetComponentInChildren<Text>(true);
    }

    void Update()
    {
        
    }
    public void StartCountDown(int time) {
        StartCoroutine(CountDown(time));
    }
    IEnumerator CountDown(int time) {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < time; i++) {
            text.text = (time - i).ToString();
            yield return new WaitForSeconds(1);
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
