using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Damage
{
     interface IPlayerDamageable
    {
         void ApplyDamage(Vector3 dir,float value);

         void HealDamage(int value);

    }
}

