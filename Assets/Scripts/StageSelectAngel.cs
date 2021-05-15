using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectAngel : MonoBehaviour {
    [SerializeField]
    float z = 0;
    [SerializeField]
    float rotateLerp = 0.1f;

    ButtonManager buttonManager;
    void Start() {
        buttonManager=GameObject.Find("Canvas/Title/Stage").GetComponent<ButtonManager>();

    }

    // Update is called once per frame
    void Update() {
        var selected=buttonManager.GetSelected();
        var screenPoint = selected.transform.position;
        screenPoint.z = z;
        var worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

        var rotate=Quaternion.LookRotation(worldPoint - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, rotateLerp);
    }
}
