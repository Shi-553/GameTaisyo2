using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerMove : MonoBehaviour ,IWindAffectable{

        Rigidbody rigid;
        [SerializeField] LayerMask mask;
        [SerializeField] float speed = 1;

        public Vector2 Dir { get; private set; }

        private void Start() {

            rigid = GetComponent<Rigidbody>();

            LookAtPlayer();
        }
        private void Update() {
            LookAtPlayer();
        }
        public void MovePlayer(Vector2 dir) {
            if (dir != Vector2.zero) {
                Dir = dir;
            }


            var forwerdRay = new Ray(transform.position - transform.forward, transform.forward);
            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

                var upFV = forwardPoints.Up.normalized;
                var rightFV = forwardPoints.Right.normalized;

                Debug.DrawLine(transform.position, transform.position + rightFV,Color.black);
                Debug.DrawLine(transform.position, transform.position + upFV,Color.black);

                var move = upFV * dir.y + rightFV * dir.x;

                rigid.AddForce(-forwardPoints.Normal * 20, ForceMode.Acceleration);

                if (dir != Vector2.zero) {
                    rigid.AddForce(move * (speed*Time.timeScale), ForceMode.VelocityChange);

                }

            }
        }
        void LookAtPlayer() {
            Ray forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

                transform.LookAt(transform.position - forwardPoints.Normal, forwardPoints.Up.normalized);

            }
        }


        public void AffectWind(Wind wind)
        {
            rigid.AddForce(wind.dir * wind.value);
        }
    }
}