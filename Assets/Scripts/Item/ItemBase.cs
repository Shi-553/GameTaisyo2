using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    public abstract class ItemBase : MonoBehaviour {
        protected void Delete() {
            Destroy(gameObject);
        }
    }
    public abstract class ImmediateItemBase : ItemBase {
        public abstract void Hit(GameObject player);
    }
    public abstract class UseableItemBase : ItemBase {
        public abstract void Use(GameObject player);
    }
}
