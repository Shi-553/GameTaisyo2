using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Item {

    public class ChangeSpeedItem : UseableItemBase {
        [SerializeField]
        bool isSpeedUp = false;

        Text speedScaleText;

        Text SpeedScaleText { get {
                if (speedScaleText == null) {
                    var textObj = GameObject.Find("Canvas/SpeedScale/Text");
                    speedScaleText = textObj.GetComponent<Text>();
                }
               return speedScaleText;
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
                }
                else {
                    scale = 0.7f;
                }
            }
            else if (Time.timeScale < 1.0f) {
                if (isSpeedUp) {
                    scale = 1.0f;
                }
            }
            else if (Time.timeScale > 1.0f) {
                if (!isSpeedUp) {
                    scale = 1.0f;
                }
            }

            if (Mathf.Approximately(Time.timeScale, scale)) {
                return;
            }
            SpeedScaleText.text = scale.ToString();

            Time.timeScale = scale;

        }
    }
}