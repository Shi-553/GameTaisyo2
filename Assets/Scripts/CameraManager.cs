using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] LayerMask mask;


    [SerializeField] float speed = 1;

    // Start is called before the first frame update
    void Start() {


        var forwerdRay = new Ray(transform.position + transform.forward * 5, transform.forward);
        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            var point = ((forwardPoints.LeftTop + forwardPoints.RightTop) / 2 +
                (forwardPoints.LeftBottom + forwardPoints.RightBottom) / 2) / 2;


            transform.position = point + forwardPoints.Normal * 10;

            transform.LookAt(point, forwardPoints.Up.normalized);
            aftQ = transform.up;
        }
    }
    Vector3 aftQ;
    void Update() {
        //transform.rotation = aftQ;

        var forwerdRay = new Ray(transform.position + transform.forward * 5, transform.forward);


        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            Debug.DrawRay(forwerdHit.point, forwardPoints.Normal * 10, Color.red);
            Debug.DrawRay(forwerdHit.point, forwardPoints.Normal1 * 10);
            Debug.DrawRay(forwerdHit.point, forwardPoints.Normal2 * 10);


            transform.position = Vector3.Lerp(transform.position, forwerdHit.point + forwardPoints.Normal * 10,0.01f);

            var firstCenter = forwardPoints.LeftTop + forwardPoints.LeftBottom / 2;
            var lastCenter = forwardPoints.RightTop + forwardPoints.RightBottom / 2;

            transform.position = Vector3.Lerp(transform.position, forwerdHit.point + forwardPoints.Normal * 10, 0.01f);
            // forwerdRay = new Ray(transform.position + transform.forward * 5, transform.forward);
            // if (Physics.Raycast(forwerdRay, out forwerdHit, Mathf.Infinity, mask)) {
            //      forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);
            //
            // var befQ = transform.rotation;
            aftQ =Vector3.Lerp(aftQ, forwardPoints.Up.normalized,0.01f);

            transform.LookAt(forwerdHit.point, aftQ);
            // aftQ = transform.rotation;

            // }

            var add = ((forwardPoints.TopWidth) +
                (forwardPoints.BottomWidth)) / 2;
            transform.position += add.normalized * speed;


            // transform.rotation = Quaternion.Lerp(befQ, aftQ, 0.01f);
        }
    }
}
