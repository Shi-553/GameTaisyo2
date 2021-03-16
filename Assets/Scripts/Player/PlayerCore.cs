﻿using Item;
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

        public void ApplyDamage(Vector3 knockback)
        {
            hp--;
            GetComponent<Rigidbody>().AddForce(knockback,ForceMode.VelocityChange);
        }

        public void HealDamage(int value)
        {
            hp+=value;
        }
        public void UseItem()
        {
            if (currentItemIndex == -1) {
                return;
            }
            useableItems[currentItemIndex].Use(gameObject);
            useableItems.RemoveAt(currentItemIndex);
            currentItemIndex--;
        }
        public void NextItem()
        {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex + 1) % useableItems.Count;
        }
        public void PrevItem()
        {
            if (currentItemIndex == -1) {
                return;
            }
            currentItemIndex = (currentItemIndex - 1) % useableItems.Count;
        }



        new Renderer renderer;
        bool isVisible = false;
        void Start() {
            renderer = GetComponent<Renderer>();


        }

        private void OnCollisionEnter(Collision collision) {
            var p = collision.gameObject.GetComponent<IOperatedPlayerObject>();
            if (p != null) {
                p.Hit();
            }
            var immediateItem = collision.gameObject.GetComponent<ImmediateItemBase>();
            if (immediateItem != null) {
                immediateItem.Hit(gameObject);
            }
            var useableItem = collision.gameObject.GetComponent<UseableItemBase>();
            if (useableItem != null) {
                if (currentItemIndex == -1) {
                    currentItemIndex = 0;
                }
                useableItems.Add(useableItem);
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
