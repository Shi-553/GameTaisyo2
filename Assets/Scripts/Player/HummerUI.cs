using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HummerUI : MonoBehaviour {
    Slider slider;
    Transform hummerImage;
    [SerializeField]
    float rejectScale = 1;
    [SerializeField]
    AudioClip rejectSe;

    private void Start() {
        slider = transform.Find("Slider").GetComponent<Slider>();
        hummerImage = transform.Find("HummerImage");

    }
    public void SetSlider(float value) {
        StartCoroutine(ActionHummer(value));
    }
    public void Reject() {
        StartCoroutine(HorizontalSwing3Action(slider.transform));
        StartCoroutine(HorizontalSwing3Action(hummerImage.transform));
        AudioManager.Instance.Play(rejectSe);
    }
    IEnumerator ActionHummer(float value) {
        var angle = hummerImage.rotation;

        yield return StartCoroutine(ActionDown());

        StartCoroutine(ActionWaitUp(angle));
        StartCoroutine(HorizontalSwing3Action(slider.transform));

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
    IEnumerator HorizontalSwing3Action(Transform t) {

        for (int i = 0; i < 3; i++) {
            var r = t.localPosition;
            r.x += rejectScale;
            t.localPosition = r;
            yield return null;
        }
        for (int i = 0; i < 6; i++) {
            var r = t.localPosition;
            r.x -= rejectScale;
            t.localPosition = r;
            yield return null;
        }
        for (int i = 0; i < 3; i++) {
            var r = t.localPosition;
            r.x += rejectScale;
            t.localPosition = r;
            yield return null;
        }
    }
}
