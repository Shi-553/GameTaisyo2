using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBlock : TransformBlock, IOperatedHummerObject
{
    int mask;
    [SerializeField] Vector2 movevalue;
    void Start()
    {
        mask = LayerMask.GetMask(new string[] { "Mebiusu" });
    }

    void Update() {
        LookAtBlock();
        MoveBlock(Vector2.zero);

    }

    void IOperatedHummerObject.Hit()
    {
        MoveBlock(movevalue);

    }

    void LookAtBlock()
    {
        Ray forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask))
        {

            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            transform.LookAt(transform.position - forwardPoints.Normal, forwardPoints.Up.normalized);

        }
    }

    public void MoveBlock(Vector2 dir)
    {


        var forwerdRay = new Ray(transform.position - transform.forward, transform.forward);
        if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask))
        {
            var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

            var upFV = forwardPoints.Up.normalized;
            var rightFV = forwardPoints.Right.normalized;

            Debug.DrawLine(transform.position, transform.position + rightFV, Color.black);
            Debug.DrawLine(transform.position, transform.position + upFV, Color.black);

            var move = upFV * dir.y + rightFV * dir.x;

            GetComponent<Rigidbody>().AddForce(-forwardPoints.Normal * 5, ForceMode.Acceleration);

            if (dir != Vector2.zero)
            {
                GetComponent<Rigidbody>().AddForce(move * (speed * Time.timeScale), ForceMode.VelocityChange);

            }

        }
    }
}
