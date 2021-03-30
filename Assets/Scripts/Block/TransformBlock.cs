using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBlock : MonoBehaviour,Item.TimeStopable {
    [SerializeField] protected Vector3 beginValue;
    [SerializeField]protected Vector3 endValue;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected AnimationCurve curve;
    

    float stoppedTime=0;
    float allDiffTime=0;


    public Vector3 GetCurrentAnimeValue() {

        var time = GetCurrentTime();

        return beginValue * (1 - time) + endValue * time;

    }
    public float GetCurrentTime() {
        if (stoppedTime != 0) {
            return curve.Evaluate((stoppedTime) * speed);
        }
        return curve.Evaluate((Time.time - allDiffTime) * speed);
    }

    public void TimeReStarted() {
        allDiffTime += stoppedTime - Time.time;
        stoppedTime = 0;
    }

    public void TimeStopped() {
        stoppedTime = Time.time;
    }
}
