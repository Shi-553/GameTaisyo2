using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    public class HeartItem : ImmediateItemBase {
        public override void Hit(GameObject player) {
            var damageble = player.GetComponent<Damage.IPlayerDamageable>();
            if (damageble != null) {
                damageble.HealDamage(1);
            }
        }
    }
}