using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    interface SpeedChangeable {
        void TimeChange(float timeScale);
    }

    public class ChangeSpeedItem : UseableItemBase {
        [SerializeField]
        float changeSecound = 3;
        [SerializeField]
        float changeTimeSpeedValue = 0.3f;

        public override bool Use(GameObject player) {
            player.GetComponent<MonoBehaviour>().StartCoroutine(TimeChange());

            return false;
        }


        IEnumerator TimeChange() {
            Time.timeScale = changeTimeSpeedValue;

            var components = new List<SpeedChangeable>();

            var a = FindObjectsOfType<Component>();
            foreach (var c in a) {
                var timeStopable = c as SpeedChangeable;

                if (timeStopable != null) {
                    timeStopable.TimeChange(changeTimeSpeedValue);
                    components.Add(timeStopable);
                }
            }
            yield return new WaitForSeconds(changeSecound);
            Time.timeScale =1;

            foreach (var c in components) {
                c.TimeChange(1);
            }
        }
    }
}