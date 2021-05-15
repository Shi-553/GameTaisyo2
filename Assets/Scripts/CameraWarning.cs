using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWarning : MonoBehaviour {
    Coroutine coroutine;

    Transform left;
    Transform right;
    [SerializeField]
    Vector3 move;
    [SerializeField]
    int speed;

    Vector3 startL;
    Vector3 startR;
    private void Start() {
        left = transform.Find("Left");
        right = transform.Find("Right");
    }

    public void StartWorning() {
        if (coroutine == null) {
            coroutine = StartCoroutine(Enumerator());
        }
    }
    public void StopWorning() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
            left.localPosition = startL;
            right.localPosition = startR;
        }
    }

    IEnumerator Enumerator() {
        startL = left.localPosition;
        startR = right.localPosition;
        var endL = startL+ move;
        var endR = startR- move;

        int time = 0;
        while (true) {
            left.localPosition = Vector3.Lerp(startL, endL,(float)time/ speed);
            right.localPosition = Vector3.Lerp(startR, endR,(float)time/ speed);

            time = (time + 1) % speed;

            yield return null;
        }
    }
}
