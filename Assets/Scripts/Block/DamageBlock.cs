using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Block {
        
    public class DamageBlock : MonoBehaviour {
        private void OnCollisionStay(Collision collision) {
            var b = collision.gameObject.GetComponent<Damage.IPlayerDamageable>();
            if (b != null) {
                b.ApplyDamage((collision.transform.position - collision.contacts[0].point).normalized * 25);
            }

        }

    }

}
