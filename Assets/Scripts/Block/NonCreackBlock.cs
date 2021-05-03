using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonCreackBlock : MonoBehaviour,IOperatedHummerObject {

    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        hummer.ApplyDamage(20, false);
    }
}
