using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Bomb : MonoBehaviour {
    int mask;

    [SerializeField]
    int atk = 3;
    [SerializeField]
    float bombRadius = 2;

    [SerializeField]
    float speed = 0.1f;

    [SerializeField]
    Vector3 dir;

    bool isExplosion = false;
    int explosionFrame = 0;
    
    [SerializeField]
    AudioClip se;

    private void Start() {

        mask = LayerMask.GetMask(new string[] { "Mebiusu" });
    }

    public void SetDirection(Vector3 direction) {
        dir = direction;
    }
    private void Update() {


            explosionFrame++;
        if (isExplosion) {
            if (explosionFrame > 4) {
                transform.GetChild(0).gameObject.SetActive(false);
            } 
            if (explosionFrame > 30) {
                Destroy(gameObject);
            }
        }
        else {
            if (explosionFrame > 60) {
                isExplosion = true;
                explosionFrame = 0;

                GetComponent<ParticleSystem>().Play();
                GetComponent<SphereCollider>().radius+= bombRadius;
                transform.GetChild(0).localScale += Vector3.one/4;

                AudioManager.Instance.Play(se);
            }

            var forwerdRay = new Ray(transform.position - transform.forward, transform.forward);
            if (Physics.Raycast(forwerdRay, out var forwerdHit, Mathf.Infinity, mask)) {
                var forwardPoints = PointDistance.GetUpRight(forwerdHit, transform.up, transform.right);
                transform.localPosition +=
                    (dir.x * forwardPoints.Right.normalized +
                    dir.y * forwardPoints.Up.normalized)
                    * speed;
            }
        }
    }
    void OnTriggerStay(Collider other) {
        if (other.tag == "Player" || other.gameObject.layer == mask) {
            return;
        }
        if (!isExplosion) {
            explosionFrame = 60;
        }

        var d = other.GetComponent<Damage.IGimmickDamageable>();
        if (d != null) {
            d.ApplyDamage(atk);
        }
    }
}
