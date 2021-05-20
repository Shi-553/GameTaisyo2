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
                DrawEffect();
            }
            return;
        }

        if (hp <= damage) {
            if (h.ApplyDamage(20)) {
                DrawEffect();
            }
        }
        else {
            h.ApplyDamage(20, false);
        }
        h = null;
    }
    void DrawEffect() {
        foreach (var c in gameObject.GetComponents<Collider>()) {
            c.enabled = false;
        }
        foreach (var c in gameObject.GetComponents<MeshRenderer>()) {
            c.enabled = false;
        }
        var cs = transform.GetComponentsInChildren<ParticleSystem>();
        foreach (var c in cs) {
            c.Play();
        }

        Destroy(gameObject, 2);
    }

    public void Hit(PlayerHummer hummer) {
        h = hummer;
    }
}
