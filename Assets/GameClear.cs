using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    [SerializeField]
    GameObject noDamageRoot;

    bool isPureNoDamage = false;
    float frame = 0;
    [SerializeField]
    float scale = 1;
    [SerializeField]
    float speed = 1;

    void Start()
    {
        frame = 0;

        isPureNoDamage = StageManager.Instance.CurrentStatus == StageStatus.PURE_NO_DAMAGE;

        if (isPureNoDamage) {
            noDamageRoot.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPureNoDamage) {
            noDamageRoot.GetComponentInChildren<Text>().transform.localScale += Vector3.one*(Mathf.Sin(frame) * scale* speed);
        }
        frame+= speed;
    }
}
