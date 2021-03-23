using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerCore : MonoBehaviour ,IDamageable
    {
        int hp = 3;
        int currentItemIndex = -1;
        List<UseableItemBase> useableItems=new List<UseableItemBase>();
        [SerializeField]
        List<GameObject> defaultUseableItems=new List<GameObject>();

        IntObjectUI heartUI;
        SlideObjectUI slideUI;

        public void ApplyDamage(Vector3 knockback)
        {
            hp--;
            GetComponent<Rigidbody>().AddForce(knockback,ForceMode.VelocityChange);
            heartUI.Remove();
            if (hp <= 0) {
                Death();
            }
        }

        public void HealDamage(int value)
        {
            hp+=value;

            heartUI.Add(value);
        }
        public void UseItem()
        {
            if (currentItemIndex == -1) {
                return;
            }
            var isDelete = useableItems[currentItemIndex].Use(gameObject);
            if (isDelete) {
                useableItems.RemoveAt(currentItemIndex);
                currentItemIndex--;

                slideUI.SetItem(useableItems[currentItemIndex].Sprite);
            }
        }
        public void NextItem()
        {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex + 1+ useableItems.Count) % useableItems.Count;
            slideUI.SetItem(useableItems[currentItemIndex].Sprite);
        }
        public void PrevItem()
        {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex - 1+ useableItems.Count) % useableItems.Count;
            slideUI.SetItem(useableItems[currentItemIndex].Sprite);
        }



        new Renderer renderer;
        bool isVisible = false;
        void Start() {
            renderer = GetComponent<Renderer>();

            useableItems = new List<UseableItemBase>();
            foreach (var item in defaultUseableItems) {
                var u = item.GetComponent<UseableItemBase>();
                u.DeleteModel();
                useableItems.Add(u);

                if (currentItemIndex == -1) {
                    currentItemIndex = 0;
                }
            }

            var canvasTrans = GameObject.Find("Canvas").transform;
            slideUI = canvasTrans.Find("Item").Find("Item").GetComponent<SlideObjectUI>();
            slideUI.SetItem(useableItems[currentItemIndex].Sprite);

            heartUI = canvasTrans.Find("Heart").GetComponent<IntObjectUI>();
            heartUI.SetMax(hp);
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
                immediateItem.DeleteModel();
            }
            var useableItem = collision.gameObject.GetComponent<UseableItemBase>();
            if (useableItem != null) {
                if (currentItemIndex == -1) {
                    currentItemIndex = 0;
                }
                useableItems.Add(useableItem);
                useableItem.DeleteModel();
                slideUI.SetItem(useableItems[currentItemIndex].Sprite);
            }


        }
        void Death() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
        }


        void Update() {
            if (!isVisible) {
                isVisible = renderer.isVisible;
            }
            if (isVisible) {
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

                // 視錐台の内側にあるか
                bool isRendered = GeometryUtility.TestPlanesAABB(planes, new Bounds(transform.position, Vector3.one * 0.1f));
                if (!isRendered) {
                    Debug.Log("yabaiyabai");
                    bool isRendered2 = GeometryUtility.TestPlanesAABB(planes, new Bounds(transform.position, transform.localScale));
                    if (!isRendered2) {
                        Death();

                    }
                }
            }



        }
    }

}
