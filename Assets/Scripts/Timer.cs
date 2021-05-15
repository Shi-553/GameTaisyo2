using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text countText;
    [SerializeField] float countTime = 4.0f;
    int count = 1;
    AudioSource gameBGM;

    [SerializeField]
    AudioClip bgm;
    [SerializeField]
    AudioClip countDownSE;

    bool isCountDownSE = false;

    void Start()
    {
        Scene.SceneManager.Instance.TimeStop();
        gameBGM = GameObject.Find("GameBGM").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCountDownSE) {
            isCountDownSE = true;
            gameBGM.PlayOneShot(countDownSE);
        }

        if (count == 0) {
            return;
        }
        if (count > 0)
        {
            count = Mathf.CeilToInt(countTime);
            countText.text = count.ToString();
            countTime -= Time.fixedDeltaTime;
        }
         if (countTime< 0)
        {

            gameObject.SetActive(false);
            countText.text = "";
            gameBGM.clip = bgm;
            gameBGM.loop = true;
            gameBGM.Play();
            Scene.SceneManager.Instance.TimeRestart();
            count = 0;
        }
    }

}
