﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour, Item.TimeStopable {
    [SerializeField] LayerMask mask;


    [SerializeField] float speed = 0.002f;

    [SerializeField] float positionRatio = 0.5f;
    [SerializeField] float positionUp = 0;

    /// <summary>
    /// 遠いときの距離
    /// </summary>
    [SerializeField] float farDistance = 25;
    /// <summary>
    /// 近いときの距離
    /// </summary>
    [SerializeField] float nearDistance = 13;

    /// <summary>
    /// 遠近切り替えにつかうBoxRayのサイズ
    /// </summary>
    [SerializeField] Vector3 thresholdRayBoxScale =new Vector3(10,5,1);

    /// <summary>
    /// 遠近切り替えの閾値
    /// </summary>
    [SerializeField]
    float thresholdDistance = 10;


     float distance;

    [SerializeField] float rayDistance = 7;


    [SerializeField] float positionLerp = 0.01f;
    [SerializeField] float pointLerp = 0.01f;
    [SerializeField] float lookAtLerp = 0.01f;

    Vector3 aftQ;

    bool isStopped = false;

    public void TimeReStarted() {
        isStopped = false;
    }

    public void TimeStopped() {
        isStopped = true;
    }

    void Start() {
        distance = farDistance;

        Application.targetFrameRate = 120;
        mask = LayerMask.GetMask(new string[] { "Mebiusu" });

        var forwerdRay = new Ray(transform.position + transform.forward * 5, transform.forward);

        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            var point = ((forwardPoints.LeftTop + forwardPoints.RightTop) / 2* positionRatio +
                (forwardPoints.LeftBottom + forwardPoints.RightBottom) / 2*(1- positionRatio)) ;


            transform.position = point + forwardPoints.Normal * distance;

            transform.LookAt(point, forwardPoints.Up.normalized);
            aftQ = transform.up;
        }
    }
    void Update() {
        if (isStopped) {
            return;
        }

        var forwerdRay = new Ray(transform.position + transform.forward * rayDistance, transform.forward);


        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


            var firstCenter = (forwardPoints.LeftTop * positionRatio + forwardPoints.LeftBottom * (1 - positionRatio));
            var lastCenter = (forwardPoints.RightTop * positionRatio + forwardPoints.RightBottom * (1 - positionRatio));

            Debug.DrawLine(firstCenter, lastCenter, Color.green);

            var point = firstCenter + Vector3.Project(forwerdHit.point - firstCenter, (lastCenter - firstCenter).normalized) + forwardPoints.Up.normalized * positionUp;

            Debug.DrawLine(firstCenter, forwerdHit.point, Color.red);
            Debug.DrawLine(firstCenter, forwerdHit.point, Color.black);

            Debug.DrawRay(point, forwardPoints.Normal * distance, Color.red);
            Debug.DrawRay(point, forwardPoints.Normal1 * distance);
            Debug.DrawRay(point, forwardPoints.Normal2 * distance);


            transform.position = Vector3.Lerp(transform.position, point + forwardPoints.Normal * distance, positionLerp * Time.timeScale);


            aftQ = Vector3.Lerp(aftQ, forwardPoints.Up.normalized, lookAtLerp * Time.timeScale);

            var p= Vector3.Lerp(forwerdHit.point, point , pointLerp * Time.timeScale);
            transform.LookAt(p, aftQ);



            transform.position += forwardPoints.Right.normalized * (speed * Time.timeScale);


        }

        ExtDebug.DrawBoxCastBox(forwerdHit.point - forwerdRay.direction * farDistance, thresholdRayBoxScale, transform.rotation, transform.forward, Mathf.Infinity, Color.red);

        if (Physics.BoxCast(forwerdHit.point-forwerdRay.direction*farDistance, thresholdRayBoxScale, transform.forward, out var boxHit, transform.rotation, Mathf.Infinity, mask)) {
            //Debug.Log(boxHit.distance);
            if (boxHit.distance > thresholdDistance) {
                distance = farDistance;
            }
            else {
                distance = nearDistance;
            }
        }
    }
}
