using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
     interface IDamageable
    {
         void ApplyDamage(Vector3 knockback);

         void HealDamage(int value);

    }
}

