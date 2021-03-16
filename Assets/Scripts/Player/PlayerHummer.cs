using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerHummer : MonoBehaviour {
        int hammerFrame = 0;
        Transform rotateCenter;

        Transform RotateCenter {
            get {
                if (rotateCenter == null) {
                    rotateCenter = transform.parent;
                }
                return rotateCenter;
            }
        }
        Quaternion startRotate;
        Vector3 startPos;

        void OnTriggerEnter(Collider other) {
            var o = other.GetComponent<IOperatedHummerObject>();
            if (o != null) {
                o.Hit();
            }
        }

        private void Update() {

            if (hammerFrame > 0) {
                hammerFrame++;
                transform.RotateAround(RotateCenter.position, RotateCenter.up, -hammerFrame / 10);

                if (hammerFrame > 80) {
                    hammerFrame = 0;
                    transform.localRotation = startRotate;
                    transform.localPosition = startPos;

                    gameObject.SetActive(false);
                }
            }
        }
        public void WieldHummer() {
            if (hammerFrame != 0) {
                return;
            }
            hammerFrame = 1;
            gameObject.SetActive(true);

            transform.RotateAround(RotateCenter.position, RotateCenter.up, 0);
            startRotate = transform.localRotation;
            startPos = transform.localPosition;
        }
    }
}