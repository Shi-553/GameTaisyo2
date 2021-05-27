using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerHummer : SingletonMonoBehaviour<PlayerHummer> {
        int atk = 1;

        Animator anime;

        [SerializeField]
        int hpMax = 100;
        int hp;
        public int Hp { get => hp;private set => hp = value; }


        HummerUI hummerUI = null;

        [SerializeField]
        AudioClip repair;

        bool isAttack = false;
        bool isParry = false;

        [SerializeField]
        AudioClip use;

        GameObject hammerRoot;

        [SerializeField]
        float hitStopTime = 0.1f;

        private void Start() {
            hp = hpMax;
            hummerUI = GameObject.Find("Hummer").GetComponent<HummerUI>();
            anime = transform.root.GetComponentInChildren<Animator>(true);

            hammerRoot = transform.parent.parent.gameObject;
            hammerRoot.SetActive(false);
            isAttack = false;
            isParry = false;

        }
        private void Update() {
            AnimatorClipInfo[] clipInfo = anime.GetCurrentAnimatorClipInfo(0);
            if (isParry && clipInfo[0].clip.name == "Fly" && hammerRoot.transform.localScale.x < 0.2f) {
                isAttack = false;
                isParry = false;
                hammerRoot.SetActive(false);
            }
        }


        void OnTriggerEnter(Collider other) {
            if (isParry) {
                return;
            }
            if (other.CompareTag("Mebiusu")) {
                anime.SetTrigger("AttackEnd");
                isAttack = false;
                isParry = true;
                AudioManager.Instance.Play(use);
                StartCoroutine(HitStop());
            }
            if (!isAttack) {
                return;
            }
            var o = other.GetComponent<IOperatedHummerObject>();
            if (o != null) {
                o.Hit(this);
                AudioManager.Instance.Play(use);
                StartCoroutine(HitStop());
            }
            var d = other.GetComponent<Damage.IGimmickDamageable>();
            if (d != null) {
                d.ApplyDamage(atk);
            }
        }
        IEnumerator HitStop() {
            anime.speed = 0;
            yield return new WaitForSecondsRealtime(hitStopTime);
            anime.speed = 1;
        }

        public void WieldHummer() {
            if (isAttack || isParry) {
                return;
            }
            if (hp == 0) {
                hummerUI.Reject();
                return;
            }
            hammerRoot.SetActive(true);
            anime.SetTrigger("Attack");
            isAttack = true;
            isParry = false;
        }

        public bool ApplyDamage(int damage, bool isPierce = true) {
            if (isParry) {
                return false;
            }
            var isAttackCurrent = isAttack;

            if (isAttack) {
                isAttack = false;
                hp -= damage;
                if (hp < 0) {
                    hummerUI.Reject();
                    hp = 0;
                    isPierce = false;
                }
                hummerUI.SetSlider(hp / (float)hpMax);
            }
            if (!isPierce) {
                anime.SetTrigger("Parry");
                isParry = true;
                isAttack = false;
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