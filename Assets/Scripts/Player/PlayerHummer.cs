using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerHummer : MonoBehaviour {
        int hummerFrame = 0;
        int hummerFrameEnd = 30;
        int atk = 1;
        Transform rotateCenter;
        
        Animator anime;

        [SerializeField]
        int hpMax = 100;
        int hp ;

        float rotate = 0;
        float rotateAdd = 0;

        HummerUI hummerUI=null;

        [SerializeField]
        AudioClip repair;

        private void Start() {
            hp = hpMax;
            hummerUI = GameObject.Find("Hummer").GetComponent<HummerUI>();
            anime =transform.parent.GetComponentInChildren<Animator>(true);
            gameObject.SetActive(false);
        }

        Transform RotateCenter {
            get {
                if (rotateCenter == null) {
                    rotateCenter = transform.parent;
                }
                return rotateCenter;
            }
        }
        Quaternion startRotate;
        Vector3 startPos;

        void OnTriggerEnter(Collider other) {
            var o = other.GetComponent<IOperatedHummerObject>();
            if (o != null) {
                o.Hit(this);
            }
            var d = other.GetComponent<Damage.IGimmickDamageable>();
            if (d != null) {
                d.ApplyDamage(atk);
            }
        }

        private void Update() {

            if (hummerFrame > 0) {
                hummerFrame += 1;
                rotate += rotateAdd;
                transform.RotateAround(RotateCenter.position, RotateCenter.up, -rotate / 10);

                if (hummerFrame > hummerFrameEnd) {
                    hummerFrame = 0;
                    transform.localRotation = startRotate;
                    transform.localPosition = startPos;

                    gameObject.SetActive(false);
                }
            }
        }
        public void WieldHummer() {
            if (hummerFrame != 0||hp == 0) {
                return;
            }
            anime.SetTrigger("Attack");
            gameObject.SetActive(true);
            rotate = 0;
            rotateAdd = 5;
            hummerFrame = 1;
            hummerFrameEnd = 30;

            transform.RotateAround(RotateCenter.position, RotateCenter.up, 0);
            startRotate = transform.localRotation;
            startPos = transform.localPosition;
        }

        public bool ApplyDamage(int damage, bool isPierce = true) {
            if (rotateAdd < 0) {
                return false;
            }

            hp -= damage;

            if (hp < 0) {
                hp = 0;
                isPierce = false;
            }
            if (!isPierce) {
                anime.SetTrigger("Parry");
                rotate -= 40;
                hummerFrameEnd = hummerFrame + 20;
                rotateAdd = -5.0f;
               
            }
            hummerUI.SetSlider(hp / (float)hpMax);


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