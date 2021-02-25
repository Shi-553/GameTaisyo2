using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour {
    int mask;

    [SerializeField] TransformBlock TransformBlock;
    [SerializeField] [Range(0.0f,1.0f)] float smoothSpeed=0.1f;


    Vector3 prevAnimeValue;

    private void Start() {
        mask = LayerMask.GetMask(new string[] { "Mebiusu" });
        prevAnimeValue = TransformBlock.GetCurrentAnimeValue();

        Set(Vector3.zero, prevAnimeValue);
    }

    private void Update() {

        var animeValue = TransformBlock.GetCurrentAnimeValue();
        Set(prevAnimeValue, animeValue);
        prevAnimeValue = animeValue;
    }

    void Set(Vector3 prev, Vector3 current) {
        Ray forwerdRay = new Ray(transform.position - transform.forward, transform.forward);


        Debug.DrawRay(forwerdRay.origin, forwerdRay.direction * 10);

        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


            var sa = current - prev;

            transform.localPosition += forwardPoints.LonggerWidth.normalized * sa.x + 
                forwardPoints.LeftHeight.normalized * sa.y+
                forwardPoints.Normal * sa.z;
        }

        forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

        if (Physics.Raycast(forwerdRay, out forwerdHit, Mathf.Infinity, mask)) {

            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


            Vector3 upFV = forwardPoints.LeftHeight.normalized;

            Quaternion OriginalRot = transform.rotation;

            transform.LookAt(transform.position - forwardPoints.Normal, upFV);
            Quaternion NewRot = transform.rotation;

            transform.rotation = Quaternion.Slerp(OriginalRot, NewRot, smoothSpeed);
        }
    }
}


