using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BeltConbeyor : MonoBehaviour {

    bool isActivate = false;

    public void Activation() {
        isActivate = true;
    }

    private void OnTriggerStay(Collider other) {
        if (!isActivate) {
            return;
        }
        if (!other.CompareTag("Block")) {
            return;
        }


        var mebiusuLayer = LayerMask.GetMask(new string[] { "Mebiusu" });
        var trans = other.transform;


        var rayFowerd = new Ray(trans.position, trans.forward);

        if (!Physics.Raycast(rayFowerd, out var hitFowerd, Mathf.Infinity, mebiusuLayer)) {
            return;
        }

        var rayBack = new Ray(hitFowerd.point + trans.forward * 3, -trans.forward);

        if (!Physics.Raycast(rayBack, out var hitBack, Mathf.Infinity, mebiusuLayer)) {
            return;
        }
        isActivate = false;

        trans.position = hitBack.point + trans.forward * hitFowerd.distance;

        var angle = trans.localEulerAngles;
        angle.x += 180;
        angle.z += 180;
        trans.localEulerAngles = angle;

        Debug.Log(trans.gameObject.name);

    }
}
