using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConbeyorSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField] BeltConbeyor beltConbeyor;

    Coroutine enumerator;
    AudioClip se;
    private void Start()
    {
        se = Resources.Load<AudioClip>("voice/SE/normal");
    }
    void IOperatedHummerObject.Hit(Player.PlayerHummer hummer) {
        if (enumerator == null) {
            enumerator = StartCoroutine(On());
            hummer.ApplyDamage(10);
            AudioManager.Instance.Play(se);
        }
    }
    

    IEnumerator On() {
        var child = transform.GetChild(0).gameObject;
        child.SetActive(false);

        beltConbeyor.Activation();

        yield return new WaitForSeconds(2);

        child.SetActive(true);

        enumerator = null;
    }
}

