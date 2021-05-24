using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HummerUI : MonoBehaviour {
    Slider slider;
    Transform hummerImage;
    private void Start() {
        slider = transform.Find("Slider").GetComponent<Slider>();
        hummerImage = transform.Find("HummerImage");

    }
    public void SetSlider(float value) {
        StartCoroutine(ActionHummer(value));
    }
    IEnumerator ActionHummer(float value) {
        var angle = hummerImage.rotation;

        yield return StartCoroutine(ActionDown());

        StartCoroutine(ActionWaitUp(angle));

        for (int i = 0; i < 3; i++) {
            slider.value = Mathf.Lerp(slider.value, value, (float)i / 3);
            yield return null;
        }

        slider.value = value;
    }
    IEnumerator ActionDown() {
        for (int i = 0; i < 3; i++) {
            var r = hummerImage.localEulerAngles;
            r.z -= 40.0f / 3;
            hummerImage.localEulerAngles = r;
            yield return null;
        }
    }
    IEnumerator ActionWaitUp(Quaternion angle) {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        for (int i = 0; i < 3; i++) {
            var r = hummerImage.localEulerAngles;
            r.z += 40.0f / 3;
            hummerImage.localEulerAngles = r;
            yield return null;
        }
        hummerImage.rotation = angle;

    }
}
