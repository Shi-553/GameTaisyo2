using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class PlayerInput : MonoBehaviour {
        PlayerMove playerMove;
        PlayerHummer playerHummer;
        PlayerCore playerCore;
        // Start is called before the first frame update
        void Start() {
            playerHummer=transform.GetComponentInChildren<PlayerHummer>(true);
            playerMove = GetComponent<PlayerMove>();
            playerCore = GetComponent<PlayerCore>();
        }

        // Update is called once per frame
        void Update() {
            if(Time.timeScale==0)
            {
                return;
            }
            if (Input.GetButtonDown("Hummer")) {
                playerHummer.WieldHummer();
            }
            if (Input.GetButtonDown("UseItem")) {
                playerCore.UseItem();
            }
            if (Input.GetButtonDown("NextItem")) {
                playerCore.NextItem();
            }
            if (Input.GetButtonDown("PrevItem")) {
                playerCore.PrevItem();
            }

            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            var dir = new Vector2(h, v).normalized;
            playerMove.MovePlayer(dir);
        }
    }
}