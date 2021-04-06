using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField]
    List<TransformBlock> list;
    void IOperatedHummerObject.Hit() {
        foreach (var block in list) {
            if (block.isStopped) {
                block.TimeReStarted();
            }
            else {
                block.TimeStopped();
            }
        }
        var child = transform.GetChild(0);
        var angles = child.localEulerAngles;
        angles.y *= -1;
        child.localEulerAngles = angles;
    }
}
