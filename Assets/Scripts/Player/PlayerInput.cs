using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerInput : MonoBehaviour {
        PlayerMove playerMove;
        PlayerHummer playerHummer;
        // Start is called before the first frame update
        void Start() {
            playerHummer=transform.GetComponentInChildren<PlayerHummer>(true);
            playerMove = GetComponent<PlayerMove>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetButtonDown("Hummer")) {
                playerHummer.WieldHummer();
            }

            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            var dir = new Vector2(h, v).normalized;
            playerMove.MovePlayer(dir);
        }
    }
}