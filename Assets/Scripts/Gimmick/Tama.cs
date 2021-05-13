using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tama : MonoBehaviour
{
    private void Start() {
        StartCoroutine(DeathTime());
    }
    IEnumerator DeathTime() {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider collider) {
        var b = collider.gameObject.GetComponent<Damage.IPlayerDamageable>();
        if (b != null) {
            b.ApplyDamage((collider.transform.position - transform.position).normalized * 25);

        }
        Destroy(gameObject);
    }

}
