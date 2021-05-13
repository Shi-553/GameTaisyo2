using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PushKeepSwitch : MonoBehaviour
{
    [SerializeField]
    List<GameObject> list;

    int count = 0;
    private void Start() {
        count = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Block") && !other.CompareTag("Player")) {
            return;
        }
        if (count == 0) {
            foreach (var obj in list) {
                obj.SetActive(false);
            }
        }
        count++;
    }
    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Block") && !other.CompareTag("Player")) {
            return;
        }
        count--;

        if (count == 0) {
            foreach (var obj in list) {
                obj.SetActive(true);
            }
        }
    }
}
