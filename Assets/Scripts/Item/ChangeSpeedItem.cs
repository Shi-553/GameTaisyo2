﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    interface SpeedChangeable {
        void TimeChange(float timeScale);
    }

    public class ChangeSpeedItem : UseableItemBase {
        [SerializeField]
        bool isSpeedUp = false;

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

            Time.timeScale = scale;

            var components = new List<SpeedChangeable>();

            var a = FindObjectsOfType<Component>();
            foreach (var c in a) {
                var timeStopable = c as SpeedChangeable;

                if (timeStopable != null) {
                    timeStopable.TimeChange(scale);
                    components.Add(timeStopable);
                }
            }
        }
    }
}