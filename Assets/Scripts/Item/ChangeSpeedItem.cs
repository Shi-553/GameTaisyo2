using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Item {

    public class ChangeSpeedItem : MonoBehaviour {

        [SerializeField]
        AudioClip speedUpSE;
        [SerializeField]
        AudioClip speedDownSE;

        IntObjectUI speedUI;


        IntObjectUI SpeedUI {
            get {
                if (speedUI == null) {
                    speedUI = GameObject.Find("Canvas/SpeedScale/Background").GetComponent<IntObjectUI>();
                }
                return speedUI;
            }
        }

        bool isPressed = false;

        private void Update() {
            var trigger = Input.GetAxis("SpeedChange");
            if (trigger == 0) {
                isPressed = false;
                return;
            }
            if (isPressed) {
                return;
            }
            isPressed = true;

            TimeChange(trigger > 0);
        }


        void TimeChange(bool isSpeedUp) {
            var scale = Time.timeScale;

            if (Mathf.Approximately(Time.timeScale, 1.0f)) {
                if (isSpeedUp) {
                    scale = 1.5f;
                    AudioManager.Instance.Play(speedUpSE);
                    SpeedUI.Add();
                }
                else {
                    scale = 0.7f;
                    AudioManager.Instance.Play(speedDownSE);
                    SpeedUI.Remove();
                }
            }
            else if (Time.timeScale < 1.0f) {
                if (isSpeedUp) {
                    scale = 1.0f;
                    AudioManager.Instance.Play(speedUpSE);
                    SpeedUI.Add();
                }
            }
            else if (Time.timeScale > 1.0f) {
                if (!isSpeedUp) {
                    scale = 1.0f;
                    AudioManager.Instance.Play(speedDownSE);
                    SpeedUI.Remove();
                }
            }

            if (Mathf.Approximately(Time.timeScale, scale)) {
                return;
            }

            Time.timeScale = scale;

        }
    }
}