using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
     interface IPlayerDamageable
    {
         void ApplyDamage(Vector3 knockback);

         void HealDamage(int value);

    }
}

