using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField] Goal goal;
    AudioClip se;
    private void Start()
    {
        se = Resources.Load<AudioClip>("voice/SE/normal");
    }
    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        goal.Show();

        gameObject.SetActive(false);
        AudioManager.Instance.Play(se);
        hummer.ApplyDamage(10);
    }
}
