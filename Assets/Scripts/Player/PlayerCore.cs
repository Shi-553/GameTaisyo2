﻿using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Player {

    public class PlayerCore : SingletonMonoBehaviour<PlayerCore>, Damage.IPlayerDamageable {
        [SerializeField]
        int hpMax = 3;
        int hp;
        bool isDamage=false;
        public StageStatus Status { get {
                if (hp == hpMax) {
                    if (!isDamage) {
                        return StageStatus.PURE_NO_DAMAGE;
                    }
                    else {
                        return StageStatus.NO_DAMAGE;
                    }
                }
                return StageStatus.CLEAR;
            }
        }

        [SerializeField]
        int itemMax = 3;
        int currentItemIndex = 0;
        List<UseableItemBase> useableItems = new List<UseableItemBase>();
        [SerializeField]
        List<GameObject> defaultUseableItems = new List<GameObject>();

        IntObjectUI heartUI;
        SlideObjectUI slideUI;

        [SerializeField]
        float invincibleTime = 1.5f;
        float currentInvincibleTime = 0;

        CameraManager cameraManager;

        [SerializeField]
        float deathDistance = 15;
        [SerializeField]
        float deathAlertDistance = 12;
        [SerializeField]
        float deathImageAlphaMax = 0.8f;
        Image alertImage;
        [SerializeField]
        AudioClip damagese;
        [SerializeField]
        AudioClip getitem;

        float baseDistance;
        [SerializeField]
        float cameraFollowUpRate = 0.5f;

        SkinnedMeshRenderer[] renderers;

        public void ApplyDamage(Vector3 dir,float value) {
            if (currentInvincibleTime != 0) {
                return;
            }
            currentInvincibleTime = invincibleTime;
            hp--;
            isDamage = true;

            GetComponent<PlayerMove>().Damage(dir, value);
            heartUI.Remove();

            AudioManager.Instance.Play(damagese);
            if (hp == 0) {
                StageManager.Instance.GameOverStage();
            }

        }

        public void HealDamage(int value) {
            hp = Mathf.Min(hp + value, hpMax);

            heartUI.Add(value);
        }
        public void UseItem() {
            if (currentItemIndex == -1|| useableItems.Count-1<currentItemIndex) {
                return;
            }
            var isDelete = useableItems[currentItemIndex].Use(gameObject);
            if (isDelete) {
                useableItems.RemoveAt(currentItemIndex);

                SetItem();
            }
        }
        public void NextItem() {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex + 1 + itemMax) % itemMax;
            slideUI.SeIndex(currentItemIndex);
        }
        public void PrevItem() {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex - 1 + itemMax) % itemMax;
            slideUI.SeIndex(currentItemIndex);
        }
        void SetItem() {
            if (currentItemIndex == -1 ) {
                slideUI.SetItem(new List<Sprite>(), 0);
                return;
            }

            slideUI.SetItem(useableItems.Select(i => i.Sprite).ToList(), currentItemIndex);
        }
       public  void SetMaxHP(int hp) {
            hpMax = hp;
            this.hp = hp;
            heartUI.SetMax(hp, hp);
        }
       public  int GetHP() {
            return hp;
        }

        void Start() {
            isDamage = false;

            useableItems = new List<UseableItemBase>();
            foreach (var item in defaultUseableItems) {
                var u = item.GetComponent<UseableItemBase>();
                u.DeleteModel(gameObject);
                useableItems.Add(u);

                if (currentItemIndex == -1) {
                    currentItemIndex = 0;
                }
            }

            var canvasTrans = GameObject.Find("Canvas").transform;
            slideUI = canvasTrans.Find("Item").Find("Item").GetComponent<SlideObjectUI>();
            SetItem();

            heartUI = canvasTrans.Find("Heart").GetComponent<IntObjectUI>();
            SetMaxHP(hpMax);

            cameraManager = Camera.main.GetComponent<CameraManager>();
            alertImage = GameObject.Find("AlertImage").GetComponent<Image>();

            baseDistance = cameraManager.MebiusuDistance;
            renderers= transform.GetChild(0).Find("attack").Find("player").GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        private void OnCollisionEnter(Collision collision) {
            var p = collision.gameObject.GetComponent<IOperatedPlayerObject>();
            if (p != null) {
                p.Hit();
            }
        }
        private void OnTriggerEnter(Collider collision) {
            var immediateItem = collision.gameObject.GetComponent<ImmediateItemBase>();
            if (immediateItem != null) {
                immediateItem.Hit(gameObject);
                immediateItem.DeleteModel(gameObject);
                AudioManager.Instance.Play(getitem);
            }
            var useableItem = collision.gameObject.GetComponent<UseableItemBase>();
            if (useableItem != null&&useableItems.Count< itemMax && useableItems.All(item => item.Sprite.GetInstanceID() != useableItem.Sprite.GetInstanceID())) {
                if (currentItemIndex == -1) {
                    currentItemIndex = 0;
                }
                useableItems.Add(useableItem);
                useableItem.DeleteModel(gameObject);
                AudioManager.Instance.Play(getitem);
                SetItem();
            }

        }


        void Update() {
            if (currentInvincibleTime > 0) {
                currentInvincibleTime -= Time.deltaTime;
            }
            if (currentInvincibleTime < 0) {
                currentInvincibleTime = 0;
                foreach (var r in renderers) {
                    r.enabled = true;
                }
            }
            else {
                if (currentInvincibleTime % 0.5 > 0.4) {
                    foreach (var r in renderers) {
                        r.enabled = false;
                    }
                }
                else {
                    foreach (var r in renderers) {
                        r.enabled = true;
                    }
                }
            }
            var cameraOffset = (baseDistance-cameraManager.MebiusuDistance) * cameraFollowUpRate;

            var deathAlertDistanceFromOffset = deathAlertDistance - cameraOffset;
            var deathDistanceFromOffset = deathDistance - cameraOffset;

            var cameraDistance = Vector3.Distance(cameraManager.MebiusuPoint, transform.position);
            var color = alertImage.color;
            color.a = Mathf.Lerp(0, deathImageAlphaMax, (cameraDistance - deathAlertDistanceFromOffset) / (deathDistanceFromOffset - deathAlertDistanceFromOffset));
            alertImage.color = color;

            //Debug.Log(cameraOffset);
            //Debug.Log(cameraDistance);

            if (cameraDistance > deathDistanceFromOffset) {
                StageManager.Instance.GameOverStage();
                enabled = false;
            }
        }
    }

}
