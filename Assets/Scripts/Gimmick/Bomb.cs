﻿using System.Collections;
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
    int isExplosion = 0;

    private void Start() {

        mask = LayerMask.GetMask(new string[] { "Mebiusu" });
    }

    public void SetDirection(Vector3 direction) {
        dir = direction;
    }
    private void Update() {


        if (isExplosion > 0) {
            isExplosion++;
            if (isExplosion > 30) {
                Destroy(gameObject);
            }
        }
        else {

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
            Debug.Log("meb");
            return;
        }
        if (isExplosion == 0) {
            transform.localScale += Vector3.one * bombRadius;
            isExplosion++;
        }

        var d = other.GetComponent<Damage.IGimmickDamageable>();
        if (d != null) {
            d.ApplyDamage(atk);
        }
    }
}