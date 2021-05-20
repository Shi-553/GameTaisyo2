using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField]
    List<TransformBlock> list;
    
    AudioClip se;

    bool isAnime = false;
    private void Start()
    {
        se = Resources.Load<AudioClip>("voice/SE/kirikae");
        isAnime = false;
    }
    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        if (isAnime) {
            return;
        }
        AudioManager.Instance.Play(se);
        foreach (var block in list) {
            block.Change();
        }

        hummer.ApplyDamage(10);

        StartCoroutine(HitAnime());
    }

    IEnumerator HitAnime() {
        isAnime = true;

        var child = transform.GetChild(0).GetChild(1);
        var angle = child.localEulerAngles;
        var startY = angle.z;
        var endY = -angle.z;

        for (int i = 0; i < 40; i++) {
            angle.z = Mathf.LerpAngle(startY, endY, (float)i / 39);
            child.localEulerAngles = angle;
            yield return null;
        }
        angle.z = endY;
        child.localEulerAngles = angle;
        isAnime = false;
    }
}
