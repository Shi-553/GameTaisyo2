﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBlock : MonoBehaviour, Item.TimeStopable {
    [SerializeField] protected Vector3 beginValue;
    [SerializeField] protected Vector3 endValue;
    [SerializeField] protected float speed = 1;
    [SerializeField] protected AnimationCurve curve;

    public bool isStopped = false;

    float stoppedTime = 0;
    float allDiffTime = 0;

    void Awake() {
        isStopItem=isStopped;
    }

    public Vector3 GetCurrentAnimeValue() {
        if (isStopped) {
            return beginValue;
        }

        var time = GetCurrentTime();

        return beginValue * (1 - time) + endValue * time;

    }
    public float GetCurrentTime() {
        if (isStopped) {
            if (stoppedTime == 0) {
                TimeStopped();
            }
            return curve.Evaluate((stoppedTime - allDiffTime) * speed);

        }
        else {
            if (stoppedTime != 0) {
                TimeReStarted();
            }
            return curve.Evaluate((Time.time - allDiffTime) * speed);
        }
    }

    bool isStopItem = false;

    public void TimeReStarted() {
        if (!isStopItem) {
            return;
        }
        allDiffTime += Time.time - stoppedTime;
        allDiffTime = Time.time;
        stoppedTime = 0;
        isStopped = false;
        isStopItem = false;
    }

    public void TimeStopped() {
        if (isStopped) {
            return;
        }
        isStopItem = true;
        isStopped = true;
        stoppedTime = Time.time;
    }
}
