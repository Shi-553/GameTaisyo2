using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConbeyorSwitch : MonoBehaviour, IOperatedHummerObject {
    [SerializeField] BeltConbeyor beltConbeyor;

    Coroutine enumerator;
    void IOperatedHummerObject.Hit() {
        if (enumerator == null) {
            enumerator = StartCoroutine(On());
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

