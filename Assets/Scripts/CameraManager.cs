using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] LayerMask mask;


    [SerializeField] float speed = 1;

    Vector3 befPoint;
    Vector3 befN;
    // Start is called before the first frame update
    void Start() {

        befPoint = transform.up;
        befN = -transform.forward;
    }

    void Update() {


        var forwerdRay = new Ray(transform.position + transform.forward * 5, transform.forward);



        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            Debug.DrawRay(forwerdHit.point, forwardPoints.Normal * 10,Color.red);
            Debug.DrawRay(forwerdHit.point, forwardPoints.Normal1 * 10);
            Debug.DrawRay(forwerdHit.point, forwardPoints.Normal2 * 10);

            var point = (forwardPoints.LeftTop + forwardPoints.LeftBottom) / 2;


            transform.position = Vector3.Slerp(transform.position, point + forwardPoints.Normal * 10, 0.01f);


            befPoint = Vector3.Lerp(befPoint, forwardPoints.LeftHeight.normalized, 0.005f);

            transform.LookAt(forwerdHit.point, befPoint);



            transform.position += forwardPoints.LonggerWidth.normalized * speed;

        }
    }
}
