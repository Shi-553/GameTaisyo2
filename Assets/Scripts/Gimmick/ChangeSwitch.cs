using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField]
    List<TransformBlock> list;
    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        foreach (var block in list) {
            block.Change();
        }
        var child = transform.GetChild(0);
        var angles = child.localEulerAngles;
        angles.y *= -1;
        child.localEulerAngles = angles;

        hummer.ApplyDamage(10);
    }
}
