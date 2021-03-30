using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour, Item.TimeStopable, Item.SpeedChangeable {
    [SerializeField] LayerMask mask;


    [SerializeField] float speed = 0.005f;
    float speedScale=1;

    [SerializeField] float positionRatio = 0.5f;
    [SerializeField] float positionUp = 0;

    [SerializeField] float distance = 12;
    [SerializeField] float rayDistance = 6;


    [SerializeField] float positionLerp = 0.01f;
    [SerializeField] float lookAtLerp = 0.01f;
    Vector3 aftQ;

    public void TimeChange(float timeScale) {
        speedScale = timeScale;
    }

    public void TimeReStarted() {
        enabled = true;
    }

    public void TimeStopped() {
        enabled = false;
    }

    void Start() {
        speedScale = 1;
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
        //transform.rotation = aftQ;

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

            transform.position = Vector3.Lerp(transform.position, point + forwardPoints.Normal * distance , positionLerp);



            aftQ = Vector3.Lerp(aftQ, forwardPoints.Up.normalized, lookAtLerp);

            transform.LookAt(forwerdHit.point, aftQ);


            var add = (forwardPoints.TopWidth * positionRatio) +
                (forwardPoints.BottomWidth * (1 - positionRatio));

            transform.position += add.normalized * (speed* speedScale);


        }
    }
}
