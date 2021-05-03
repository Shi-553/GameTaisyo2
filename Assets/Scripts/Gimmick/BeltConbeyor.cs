using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BeltConbeyor : MonoBehaviour {
    public void Activation() {

        var mebiusuLayer = LayerMask.GetMask(new string[] { "Mebiusu" });

        var colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);
        var objects = colliders.Where(c => !c.CompareTag("Mebiusu") && !c.CompareTag("Player"));
        foreach (var obj in objects) {
            var trans = obj.transform;


            var rayFowerd = new Ray(trans.position, trans.forward);

            if (!Physics.Raycast(rayFowerd, out var hitFowerd, Mathf.Infinity, mebiusuLayer)) {
                continue;
            }

            var rayBack = new Ray(hitFowerd.point + trans.forward * 3, -trans.forward);

            if (!Physics.Raycast(rayBack, out var hitBack, Mathf.Infinity, mebiusuLayer)) {
                continue;
            }

            trans.position = hitBack.point + trans.forward * hitFowerd.distance;

            var angle = trans.localEulerAngles;
            angle.x += 180;
            trans.localEulerAngles = angle;

        }

    }
    void Start() {

    }

    void Update() {

    }
}
