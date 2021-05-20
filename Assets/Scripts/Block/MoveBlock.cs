using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : TransformBlock {
    int mask;

    [SerializeField] [Range(0.0f, 1.0f)] float smoothSpeed = 0.1f;


    Vector3 prevAnimeValue;
    float prevTime;
    Vector3 startV;
    Quaternion startR;
    Vector3 endV;
    Quaternion endR;
    bool isE = false;

    private void Start() {
        mask = LayerMask.GetMask(new string[] { "Mebiusu" });
        prevAnimeValue = GetCurrentAnimeValue();
        startV = transform.position;
        startR = transform.rotation;

        Set(Vector3.zero, prevAnimeValue);
    }

    private void Update() {

        var animeValue = GetCurrentAnimeValue();
        if (GetCurrentTime() > prevTime) {
            isE = false;
        }
        if (GetCurrentTime() < prevTime) {
            if (!isE) {
                endV = transform.position;
                endR = transform.rotation;
                isE = true;
            }
            transform.position = Vector3.Lerp(endV, startV, 1 - GetCurrentTime());
            transform.rotation = Quaternion.Lerp(endR, startR, 1 - GetCurrentTime());
        }
        else {
            Set(prevAnimeValue, animeValue);
        }
        prevTime = GetCurrentTime();
        prevAnimeValue = animeValue;
    }

    void Set(Vector3 prev, Vector3 current) {
        Ray forwerdRay = new Ray(transform.position - transform.forward, transform.forward);


        Debug.DrawRay(forwerdRay.origin, forwerdRay.direction * 10);

        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


            var sa = current - prev;

            transform.localPosition += forwardPoints.Right.normalized * sa.x +
                forwardPoints.Up.normalized * sa.y +
                forwardPoints.Normal * sa.z;
        }

        forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

        if (Physics.Raycast(forwerdRay, out forwerdHit, Mathf.Infinity, mask)) {

            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


            Vector3 upFV = forwardPoints.Up.normalized;

            Quaternion OriginalRot = transform.rotation;

            transform.LookAt(transform.position - forwardPoints.Normal, upFV);
            Quaternion NewRot = transform.rotation;

            transform.rotation = Quaternion.Slerp(OriginalRot, NewRot, smoothSpeed);
        }
    }
}


