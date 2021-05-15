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
    int time;
    [SerializeField]
    int stopTime;

    bool isEnd = false;

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
        isEnd = true;
    }

    IEnumerator Enumerator() {
        var startL = left.localPosition;
        var startR = right.localPosition;
        var endL = startL + move;
        var endR = startR - move;

        int t = 0;
        int st = 0;

        while (true) {
            left.localPosition = Vector3.Slerp(startL, endL, (float)t / time);
            right.localPosition = Vector3.Slerp(startR, endR, (float)t / time);

            t++;

            if (t > time) {
                st++;
                if (st > stopTime) {
                    t = 0;
                    st = 0;
                    if (isEnd) {
                        left.localPosition = startL;
                        right.localPosition = startR;
                        coroutine = null;
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }
}
