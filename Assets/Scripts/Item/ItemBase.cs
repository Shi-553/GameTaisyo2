using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    public abstract class ItemBase : MonoBehaviour {
        public void DeleteModel() {
            gameObject.SetActive(false);
        }
    }
    public abstract class ImmediateItemBase : ItemBase {
        public abstract void Hit(GameObject player);
    }
    public abstract class UseableItemBase : ItemBase {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns>isDelete</returns>
        public abstract bool Use(GameObject player);

        [SerializeField]
        Sprite sprite;
        public Sprite Sprite { get { return sprite; } }
    }
}
