using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackBlock : MonoBehaviour, Damage.IGimmickDamageable, IOperatedHummerObject {
    [SerializeField]
    int hp = 1;

    PlayerHummer h=null;
    public void ApplyDamage(int damage) {
        if (h == null) {
            if (hp <= damage) {
                Destroy(gameObject);
            }
            return;
        }

        if (hp <= damage) {
            if (h.ApplyDamage(20)) {
                Destroy(gameObject);
            }
        }
        else {
            h.ApplyDamage(20, false);
        }
        h = null;
    }

    public void Hit(PlayerHummer hummer) {
        h = hummer;
    }
}
