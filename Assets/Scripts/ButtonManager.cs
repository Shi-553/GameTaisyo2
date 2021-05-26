using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField]
    Transform selectImage;
    Vector3 selectImageDiff;
    Vector3 selectImageAngle;
    [SerializeField]
    bool isSelectHummer = true;

    [SerializeField]
    Vector3 selectScale;

    [SerializeField]
    bool isChangeColor = true;
    Color selectColor = Color.white;
    Color unselectColor = new Color(150 / 255.0f, 150 / 255.0f, 150 / 255.0f, 1);

    int selectIndex = 0;
    bool pressed = true;

    bool isSubmit = false;

    [SerializeField] bool swapVH = false;

    [SerializeField] bool reverseV = false;
    [SerializeField] bool reverseH = false;

    [SerializeField] int columnMax = 0;

    [SerializeField]
    AudioClip se;
    [Serializable] public class MyEvent : UnityEvent<GameObject> { }
    [SerializeField] MyEvent action;

    float sinFrame = 0;
    [SerializeField]
    float selectImageMoveScale = -0.6f;
    [SerializeField]
    float selectImageMoveSpeed = 0.1f;

    bool isNext = false;
    float nextTime = 0;

    bool firstStart = true;

    public GameObject GetSelected() {
        return transform.GetChild(selectIndex).gameObject;
    }
    public void ButtonReset() {
        Start();

        isSubmit = false;
    }
    public void ButtonEnd() {
        isSubmit = true;
    }

    private void Start() {
        var selected = transform.GetChild(selectIndex);

        if (firstStart) {
            if (selectImage != null) {
                selectImageDiff = selectImage.position - selected.position;
                if (isSelectHummer) {
                    selectImageAngle = selectImage.localEulerAngles;
                }
            }
        }
        else {
            if (selectImage != null) {
                selectImage.position = selected.position + selectImageDiff;

                if (isSelectHummer) {
                    selectImage.localEulerAngles = selectImageAngle;
                }
            }
        }
        selected.localScale += selectScale;

        if (isChangeColor) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).GetComponent<Image>().color = unselectColor;
            }
            selected.GetComponent<Image>().color = selectColor;
        }
        isNext = false;
        nextTime = 0;

        firstStart = false;
    }


    void Update() {
        if (isSubmit) {
            return;
        }
        if (isSelectHummer && selectImage != null) {
            var selectPos = selectImage.localPosition;
            selectPos.y += Mathf.Sin(sinFrame) * selectImageMoveScale;
            sinFrame += selectImageMoveSpeed;
            selectImage.localPosition = selectPos;
        }

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        if (swapVH) {
            var temp = v;
            v = h;
            h = temp;
        }

        if (!pressed) {
            if (v != 0 || h != 0) {
                pressed = true;
                AudioManager.Instance.Play(se);
                var selected = transform.GetChild(selectIndex);
                selected.localScale -= selectScale;
                if (isChangeColor) {
                    selected.GetComponent<Image>().color = unselectColor;
                }
            }

            var vVal = reverseV ? 1 : -1;
            var hVal = reverseH ? 1 : -1;
            if (v > 0) {
                selectIndex -= 1 * vVal;
            }
            if (v < 0) {
                selectIndex += 1 * vVal;
            }
            if (h > 0) {
                selectIndex -= columnMax * hVal;
            }
            if (h < 0) {
                selectIndex += columnMax * hVal;
            }
            if (selectIndex < 0) {
                selectIndex = (selectIndex + transform.childCount) % transform.childCount;
            }
            if (selectIndex > transform.childCount - 1) {
                selectIndex = selectIndex % transform.childCount;
            }

            if (pressed) {
                sinFrame = 0;
                var selected = transform.GetChild(selectIndex);

                if (selectImage != null) {
                    selectImage.position = selected.position + selectImageDiff;
                }
                selected.localScale += selectScale;
                if (isChangeColor) {
                    selected.GetComponent<Image>().color = selectColor;
                }
            }
        }

        if (v == 0 && h == 0) {
            pressed = false;
            isNext = false;
            nextTime = 0;
        }
        else {
            nextTime += Time.unscaledDeltaTime;
            if (!isNext && nextTime > 0.4f) {
                pressed = false;
                nextTime = 0;
                isNext = true;
            }
            if (isNext && nextTime > 0.2f) {
                nextTime = 0;
                pressed = false;
            }
        }

        if (Input.GetButtonDown("Submit")) {
            StartCoroutine(ActionHummer());
        }
    }
    IEnumerator ActionHummer() {
        isSubmit = true;
        if (isSelectHummer && selectImage != null) {
            var angle = selectImage.rotation;
            for (int i = 0; i < 4; i++) {
                var r = selectImage.localEulerAngles;
                r.z -= 70.0f / 4;
                selectImage.localEulerAngles = r;
                yield return null;
            }
            StartCoroutine(ActionWaitUp(angle));
        }

        transform.GetChild(selectIndex).localScale -= selectScale;

        transform.GetChild(selectIndex).GetComponent<Image>().color = unselectColor;

        UnityEngine.EventSystems.BaseEventData data = new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current);
        transform.GetChild(selectIndex).GetComponent<Button>().OnSubmit(data);
        action.Invoke(transform.GetChild(selectIndex).gameObject);
    }
    IEnumerator ActionWaitUp(Quaternion angle) {
        yield return new WaitForSecondsRealtime(0.1f);
        transform.GetChild(selectIndex).localScale += selectScale;

        transform.GetChild(selectIndex).GetComponent<Image>().color = selectColor;
        for (int i = 0; i < 4; i++) {
            var r = selectImage.localEulerAngles;
            r.z += 70.0f / 4;
            selectImage.localEulerAngles = r;
            yield return null;
        }
        selectImage.rotation = angle;

    }
}
