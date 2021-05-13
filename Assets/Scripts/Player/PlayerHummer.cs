using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerHummer : MonoBehaviour {
        int atk = 1;

        Animator anime;

        [SerializeField]
        int hpMax = 100;
        int hp;


        HummerUI hummerUI = null;

        [SerializeField]
        AudioClip repair;

        bool isAttack = false;

        private void Start() {
            hp = hpMax;
            hummerUI = GameObject.Find("Hummer").GetComponent<HummerUI>();
            anime = transform.root.GetComponentInChildren<Animator>(true);
            isAttack = false;
        }


        void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Mebiusu")) {
                anime.SetTrigger("AttackEnd");
                isAttack = false;
            }
            if (!isAttack) {
                return;
            }
            var o = other.GetComponent<IOperatedHummerObject>();
            if (o != null) {
                o.Hit(this);
            }
            var d = other.GetComponent<Damage.IGimmickDamageable>();
            if (d != null) {
                d.ApplyDamage(atk);
            }
        }

        public void WieldHummer() {
            if (hp == 0) {
                return;
            }
            anime.SetTrigger("Attack");
            isAttack = true;
        }

        public bool ApplyDamage(int damage, bool isPierce = true) {
            var isAttackCurrent = isAttack;

            if (isAttack) {
                isAttack = false;
                hp -= damage;
                if (hp < 0) {
                    hp = 0;
                    isPierce = false;
                }
                hummerUI.SetSlider(hp / (float)hpMax);
            }
            if (!isPierce) {
                anime.SetTrigger("Parry");
            }

            if (!isAttackCurrent) {
                return false;
            }

            return isPierce;
        }

        public void Repair(int value) {
            hp += value;
            AudioManager.Instance.Play(repair);
            if (hp > hpMax) {
                hp = hpMax;
            }
            hummerUI.SetSlider(hp / (float)hpMax);
        }
    }
}