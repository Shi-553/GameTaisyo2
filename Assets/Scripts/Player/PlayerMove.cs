using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerMove : MonoBehaviour, Item.SpeedChangeable {

        new Rigidbody rigidbody;
        [SerializeField] LayerMask mask;
        [SerializeField] float speed = 1;
        float speedScale=1;

        private void Start() {
            speedScale = 1;

            rigidbody = GetComponent<Rigidbody>();

            LookAtPlayer();
        }
        private void Update() {
            LookAtPlayer();
        }
        public void MovePlayer(Vector2 dir) {
            var forwerdRay = new Ray(transform.position - transform.forward, transform.forward);
            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

                var upFV = forwardPoints.Up.normalized;
                var rightFV = forwardPoints.Right.normalized;


                var move = upFV * dir.y + rightFV * dir.x;

                if (dir != Vector2.zero) {
                    rigidbody.AddForce(move * (speed* speedScale), ForceMode.VelocityChange);

                }

                rigidbody.AddForce(-forwardPoints.Normal * 10, ForceMode.Acceleration);
            }
        }
        void LookAtPlayer() {
            Ray forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);


                var upFV = forwardPoints.Up.normalized;

               // var befRoatte = transform.rotation;

                var look = Vector3.Slerp(transform.position + transform.forward, transform.position - forwardPoints.Normal, 0.5f);

                transform.LookAt(look, upFV);

                //var sa = Quaternion.Angle(befRoatte, transform.rotation);
                //if (sa > 20) {
                //    transform.rotation = befRoatte;
                //}
                //Debug.Log(sa);


            }
        }

        public void TimeChange(float timeScale) {
            speedScale = timeScale;
        }
    }
}