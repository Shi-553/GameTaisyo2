using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    public class BombItem : UseableItemBase {
        [SerializeField]
        GameObject bombPrefab;

        public override bool Use(GameObject player) {
            var bomb=Instantiate(bombPrefab);

            var dir = player.GetComponent<Player.PlayerMove>().Dir;

            bomb.transform.position = player.transform.position;
            bomb.transform.rotation = player.transform.rotation;

            bomb.GetComponent<Bomb>().SetDirection(new Vector2(1,0));

            return true;
        }
    }
}