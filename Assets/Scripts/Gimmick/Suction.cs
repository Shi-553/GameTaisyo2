using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Suction : MonoBehaviour {
    [SerializeField]
    float suctionPower = 1000;
    [SerializeField]
    float suctionThresholdSpeed = 1;

    private void OnTriggerStay(Collider other) {
        if (!other.CompareTag("Block")) {
            return;
        }
        var rig = other.GetComponent<Rigidbody>();
        if (rig == null) {
            return;
        }
        var speed = rig.velocity.magnitude;

        if (speed < suctionThresholdSpeed && speed > 0.1f) {
            rig.AddForce((transform.position - other.transform.position).normalized * suctionPower, ForceMode.Acceleration);
        }
    }
}
