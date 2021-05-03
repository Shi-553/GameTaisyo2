using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IOperatedHummerObject
{
    [SerializeField] Door door;

    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        door.OpenDoor();
        gameObject.SetActive(false);

        hummer.ApplyDamage(10);
    }
}
