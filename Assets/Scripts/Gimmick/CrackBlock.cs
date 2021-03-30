using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackBlock : MonoBehaviour,Damage.IGimmickDamageable
{
    [SerializeField]
    int hp = 1;
    public void ApplyDamage(int damage) {
        if (hp <= damage) {
            Destroy(gameObject);
        }
    }
}
