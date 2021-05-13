using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IOperatedHummerObject
{
    [SerializeField] Door door;

    AudioClip se;
    private void Start()
    {
     se = Resources.Load<AudioClip>("voice/SE/normal");
    }

    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        door.OpenDoor();
        gameObject.SetActive(false);
        AudioManager.Instance.Play(se);
        hummer.ApplyDamage(10);
    }
}
