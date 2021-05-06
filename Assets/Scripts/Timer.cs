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

    // Start is called before the first frame update
    void Start()
    {
        Scene.SceneManager.Instance.TimeStop();
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 0) {
            return;
        }
        if (Input.GetButtonDown("Submit")) {
            countTime = 0;
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
            Scene.SceneManager.Instance.TimeRestart();
            count = 0;
        }
    }

}
