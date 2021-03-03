using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Block
{
    public class DamageBlock : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            var b = collision.gameObject.GetComponent<Player.IDamageable>();
            if (b != null)
            {
                b.ApplyDamage((collision.transform.position - collision.contacts[0].point).normalized * 25);

            }

        }


    }

}
