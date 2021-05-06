using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Item {

    public class ChangeSpeedItem : UseableItemBase {
        [SerializeField]
        bool isSpeedUp = false;

        [SerializeField]
        AudioClip se;

        IntObjectUI speedUI;


        IntObjectUI SpeedUI { get {
                if (speedUI == null) {
                    speedUI = GameObject.Find("Canvas/SpeedScale/Background").GetComponent<IntObjectUI>();
                }
               return speedUI;
            } 
        }


        public override bool Use(GameObject player) {
            TimeChange(isSpeedUp);

            return false;
        }


        void TimeChange(bool isSpeedUp) {
            var scale = Time.timeScale;

            if (Mathf.Approximately(Time.timeScale, 1.0f)) {
                if (isSpeedUp) {
                    scale = 1.5f;
                    AudioManager.Instance.Play(se);
                    SpeedUI.Add();
                }
                else {
                    scale = 0.7f;
                    AudioManager.Instance.Play(se);
                    SpeedUI.Remove();
                }
            }
            else if (Time.timeScale < 1.0f) {
                if (isSpeedUp) {
                    scale = 1.0f;
                    AudioManager.Instance.Play(se);
                    SpeedUI.Add();
                }
            }
            else if (Time.timeScale > 1.0f) {
                if (!isSpeedUp) {
                    scale = 1.0f;
                    AudioManager.Instance.Play(se);
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