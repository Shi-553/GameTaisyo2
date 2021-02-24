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


        var forwerdRay = new Ray(transform.position + transform.forward*5, transform.forward);



        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            Debug.DrawRay(forwerdHit.point, forwerdHit.normal * 10);

            var point = (forwardPoints[1].P1 + forwardPoints[1].P2) / 2;

            if (Vector3.Distance(transform.position, point + forwerdHit.normal * 10) < 5) {
                befN = forwerdHit.normal;
                transform.position = Vector3.Slerp(transform.position, point + forwerdHit.normal * 10, 0.01f);
            }
            else {
                transform.position = Vector3.Slerp(transform.position, point + befN * 10, 0.01f);
            }

            befPoint = Vector3.Lerp(befPoint, forwardPoints[1].NomalD, 0.005f);

            transform.LookAt(forwerdHit.point, befPoint);



            Vector3 rightFV = forwardPoints[0].NomalD;
            transform.position += rightFV * speed;

        }
    }
}
