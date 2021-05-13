using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField]
    List<TransformBlock> list;
    
    AudioClip se;
    private void Start()
    {
        se = Resources.Load<AudioClip>("voice/SE/kirikae");
    }
    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        AudioManager.Instance.Play(se);
        foreach (var block in list) {
            block.Change();
        }
        var child = transform.GetChild(0);
        var angles = child.localEulerAngles;
        angles.y *= -1;
        child.localEulerAngles = angles;

        hummer.ApplyDamage(10);
    }
}
