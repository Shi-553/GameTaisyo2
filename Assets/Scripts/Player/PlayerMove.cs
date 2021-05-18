using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class PlayerMove : MonoBehaviour, IWindAffectable {

        Rigidbody rigid;
        [SerializeField] LayerMask mask;
        [SerializeField] float speed = 1;
        [SerializeField] float moveForceMultiplier;
        [SerializeField] float gravity;

        public Vector2 Dir { get; private set; }
        public Vector3 Normal { get; private set; }


        [SerializeField]
        float inclineAngleScale;
        [SerializeField]
        Vector3 inclineAngleX;
        [SerializeField]
        Vector3 inclineAngleY;
        Transform child;
        Vector3 childStartAngle;

        Transform cameraTransform;

#if UNITY_EDITOR
        Text speedText;
#endif
        private void Start() {

            rigid = GetComponent<Rigidbody>();
            child = transform.GetChild(0);
            childStartAngle = child.localEulerAngles;
            cameraTransform = Camera.main.transform;

#if UNITY_EDITOR
            speedText = GameObject.Find("Canvas").transform.Find("Speed").GetComponent<Text>();
            speedText.gameObject.SetActive(true);
#endif
            LookAtPlayer();
        }
        private void Update() {
            LookAtPlayer();
#if UNITY_EDITOR
            speedText.text = (rigid.velocity.magnitude).ToString("0");
#endif
        }
        public void MovePlayer(Vector2 dir) {
            if (dir != Vector2.zero) {
                Dir = dir;
            }
            var childAngle = childStartAngle;
            childAngle += dir.x * inclineAngleScale * inclineAngleX;
            childAngle += dir.y * inclineAngleScale * inclineAngleY;
            child.localEulerAngles = childAngle;

            var forwerdRay = new Ray(transform.position - transform.forward, transform.forward);
            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);
                Normal = forwardPoints.Normal;

                var upFV = Vector3.ProjectOnPlane(cameraTransform.up, forwardPoints.Normal).normalized;
                var rightFV = Vector3.ProjectOnPlane(cameraTransform.right, forwardPoints.Normal).normalized;


                Debug.DrawLine(transform.position, transform.position + rightFV, Color.black);
                Debug.DrawLine(transform.position, transform.position + upFV, Color.black);
                Debug.DrawLine(forwerdHit.point, forwerdHit.point + forwardPoints.Normal, Color.red);

                var projectDir = (upFV * dir.y + rightFV * dir.x).normalized;
                var move = projectDir * (speed * Time.timeScale);

                rigid.AddForce(moveForceMultiplier * (move - rigid.velocity), ForceMode.Force);

                rigid.AddForce((forwerdHit.point - transform.position).magnitude * -forwardPoints.Normal * gravity, ForceMode.Acceleration);

            }
        }
        void LookAtPlayer() {
            Ray forwerdRay = new Ray(transform.position - transform.forward, transform.forward);

            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {

                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);

                transform.LookAt(transform.position - forwardPoints.Normal, forwardPoints.Up.normalized);

            }
        }
        public void Damage(Vector3 dir,float value) {
            rigid.AddForce(Vector3.ProjectOnPlane(dir, Normal).normalized * value, ForceMode.VelocityChange);
        }


        public void AffectWind(Wind wind) {
            rigid.AddForce(Vector3.ProjectOnPlane(wind.dir,Normal).normalized * wind.value);
        }
    }
}