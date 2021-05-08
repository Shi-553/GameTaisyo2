using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Player {

    public class PlayerCore : MonoBehaviour, Damage.IPlayerDamageable {
        [SerializeField]
        int hpMax = 3;
        int hp;

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

        float baseDistance;
        [SerializeField]
        float cameraFollowUpRate = 0.5f;

        public void ApplyDamage(Vector3 knockback) {
            if (currentInvincibleTime != 0) {
                return;
            }
            currentInvincibleTime = invincibleTime;
            hp--;
            GetComponent<Rigidbody>().AddForce(knockback, ForceMode.VelocityChange);
            heartUI.Remove();

            AudioManager.Instance.Play(damagese);
            if (hp == 0) {
                Death();
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
                currentItemIndex--;

                SetItem();
            }
        }
        public void NextItem() {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex + 1 + itemMax) % itemMax;
            SetItem();
        }
        public void PrevItem() {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex - 1 + itemMax) % itemMax;
            SetItem();
        }
        void SetItem() {
            if (currentItemIndex == -1 || useableItems.Count == 0) {
                slideUI.SetItem(new List<Sprite>(), 0);
                return;
            }

            slideUI.SetItem(useableItems.Select(i => i.Sprite).ToList(), currentItemIndex);
        }
        void Start() {
            hp = hpMax;

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

            cameraManager = Camera.main.GetComponent<CameraManager>();
            alertImage = GameObject.Find("AlertImage").GetComponent<Image>();

            baseDistance = cameraManager.MebiusuDistance;
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
            }
            var useableItem = collision.gameObject.GetComponent<UseableItemBase>();
            if (useableItem != null&&useableItems.Count< itemMax && useableItems.All(item => item.Sprite.GetInstanceID() != useableItem.Sprite.GetInstanceID())) {
                if (currentItemIndex == -1) {
                    currentItemIndex = 0;
                }
                useableItems.Add(useableItem);
                useableItem.DeleteModel(gameObject);
                SetItem();
            }


        }
        void Death() {
            Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.GAMEOVER, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            Time.timeScale = 0;
        }


        void Update() {
            if (currentInvincibleTime > 0) {
                currentInvincibleTime -= Time.deltaTime;
            }
            if (currentInvincibleTime < 0) {
                currentInvincibleTime = 0;
                GetComponent<MeshRenderer>().enabled = true;
            }
            else {
                if (currentInvincibleTime % 0.5 > 0.4) {
                    //GetComponent<MeshRenderer>().enabled = false;
                }
                else {
                    //GetComponent<MeshRenderer>().enabled = true;
                }
            }
            var cameraOffset = (baseDistance-cameraManager.MebiusuDistance) * cameraFollowUpRate;

            var deathAlertDistanceFromOffset = deathAlertDistance - cameraOffset;
            var deathDistanceFromOffset = deathDistance - cameraOffset;

            var cameraDistance = Vector3.Distance(cameraManager.MebiusuPoint, transform.position);
            var color = alertImage.color;
            color.a = Mathf.Lerp(0, deathImageAlphaMax, (cameraDistance - deathAlertDistanceFromOffset) / (deathDistanceFromOffset - deathAlertDistanceFromOffset));
            alertImage.color = color;

            Debug.Log(cameraOffset);
            Debug.Log(cameraDistance);

            if (cameraDistance > deathDistanceFromOffset) {
                Death();
                enabled = false;
            }
        }
    }

}
