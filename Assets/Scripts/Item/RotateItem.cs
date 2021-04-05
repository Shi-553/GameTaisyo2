using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour {
    float rotateSize = 15;
    [SerializeField]
    bool x = false, y = false, z = true;

    AnimationCurve curve = new AnimationCurve(
           new Keyframe[]{
        new Keyframe(0,0),
        new Keyframe(0.2f,1),
        new Keyframe(0.5f,1),
        new Keyframe(0.7f,0 )
               }
           );
    void Start() {
        curve.postWrapMode = WrapMode.PingPong;
        curve.preWrapMode = WrapMode.PingPong;
    }

    float oldVal = 0;

    void Update() {
        var l = transform.localEulerAngles;

        var val = (curve.Evaluate(Time.time) * rotateSize) ;
        if (x) {
            l.x += val - oldVal;
        }
        if (y) {
            l.y += val - oldVal;
        }
        if (z) {
            l.z += val - oldVal;
        }
        transform.localEulerAngles = l;

        oldVal = val;
    }
}
