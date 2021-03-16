using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    interface TimeStopable {
        void TimeStopped();
        void TimeReStarted();
    }
    public class StopTimeItem : UseableItemBase {
        [SerializeField]
        float stopSecound = 3;

        public override void Use(GameObject player) {
            player.GetComponent<MonoBehaviour>().StartCoroutine(TimeStop());
        }

        IEnumerator TimeStop() {
            var components = new List<TimeStopable>();

            var a = FindObjectsOfType<Component>();
            foreach (var c in a) {
                var timeStopable = c as TimeStopable;

                if (timeStopable != null) {
                    timeStopable.TimeStopped();
                    components.Add(timeStopable);
                }
            }
            yield return new WaitForSeconds(stopSecound);

            foreach (var c in components) {
                c.TimeReStarted();
            }
        }
    }
}