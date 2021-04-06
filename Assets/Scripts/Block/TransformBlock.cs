using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBlock : MonoBehaviour,Item.TimeStopable {
    [SerializeField] protected Vector3 beginValue;
    [SerializeField]protected Vector3 endValue;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected AnimationCurve curve;

    public bool isStopped = false;

    float stoppedTime=0;
    float allDiffTime=0;


    public Vector3 GetCurrentAnimeValue() {

        var time = GetCurrentTime();

        return beginValue * (1 - time) + endValue * time;

    }
    public float GetCurrentTime() {
        if (isStopped) {
            if (stoppedTime == 0) {
                stoppedTime = Time.time;
            }
                return curve.Evaluate((stoppedTime- allDiffTime) * speed);
        }
        return curve.Evaluate((Time.time - allDiffTime) * speed);
    }

    public void TimeReStarted() {
        allDiffTime +=Time.time-stoppedTime;
        stoppedTime = 0;
        isStopped = false;
    }

    public void TimeStopped() {
        isStopped = true;
        stoppedTime = Time.time;
    }
}
