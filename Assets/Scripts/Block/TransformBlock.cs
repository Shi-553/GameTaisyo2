using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBlock : MonoBehaviour, Item.TimeStopable {
    [SerializeField] protected Vector3 beginValue;
    [SerializeField] protected Vector3 endValue;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected AnimationCurve curve;

    [SerializeField]
    bool isStopped = false;

    float stoppedTime = 0;
    float allDiffTime = 0;

    public Vector3 GetCurrentAnimeValue() {

        var time = GetCurrentTime();

        return beginValue * (1 - time) + endValue * time;

    }
    public float GetCurrentTime() {

        if (isStopTime) {
            if (stoppedTime == 0) {
                TimeStopped();
            }
            return curve.Evaluate((stoppedTime - allDiffTime) * speed);

        }
        else {
            if (isStopped) {
                return 0;
            }
            if (stoppedTime != 0) {
                TimeReStarted();
            }
            return curve.Evaluate((Time.time - allDiffTime) * speed);
        }
    }

    public void Change() {
        isStopped = !isStopped;
    }

    bool isStopTime = false;
    public void TimeReStarted() {
        if (!isStopTime) {
            return;
        }
        allDiffTime += Time.time - stoppedTime;
        stoppedTime = 0;
        isStopTime = false;
    }

    public void TimeStopped() {
        if (isStopTime) {
            return;
        }
        isStopTime = true;
        if (!isStopped) {
            stoppedTime = Time.time;
        }
    }
}
