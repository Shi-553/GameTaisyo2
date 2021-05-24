using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCountUI : MonoBehaviour
{
    [SerializeField]
    GameObject deathRoot;

    void Start()
    {
        if (StageManager.Instance.IsDeathHpUp()) {
            deathRoot.transform.Find("DeathHpUp").gameObject.SetActive(true);
        }
        deathRoot.transform.Find("DeathCount").GetComponent<Text>().text = StageManager.Instance.GetDeathCount().ToString() + "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
