using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField]
    Transform selectImage;

    [SerializeField]
    Vector3 selectScale;

    [SerializeField]
    bool isChangeColor = true;
    Color selectColor=Color.white;
    Color unselectColor = new Color(150 / 255.0f, 150 / 255.0f, 150/ 255.0f, 1);

    int selectIndex = 0;
    bool pressed = false;

    bool isSubmit = false;

    [SerializeField] bool swapVH = false;

    [SerializeField] bool reverseV = false;
    [SerializeField] bool reverseH = false;

    [SerializeField] int columnMax = 0;
    
    [SerializeField]
    AudioClip se;
    [Serializable] public class MyEvent : UnityEvent<GameObject> { }
    [SerializeField] MyEvent action;

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

        if (selectImage != null) {
            selectImage.position = selected.position;
        }
        selected.localScale += selectScale;

        if (isChangeColor) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).GetComponent<Image>().color = unselectColor;
        }
            selected.GetComponent<Image>().color = selectColor;
        }
    }


    void Update() {
        if (isSubmit) {
            return;
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
                selectIndex -= 1* vVal;
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
                var selected = transform.GetChild(selectIndex);

                if (selectImage != null) {
                    selectImage.position = selected.position;
                }
                selected.localScale += selectScale;
                if (isChangeColor) {
                    selected.GetComponent<Image>().color = selectColor;
                }
            }
        }

        if (v == 0 && h == 0) {
            pressed = false;
        }

        if (Input.GetButtonDown("Submit")) {
            isSubmit = true;
            UnityEngine.EventSystems.BaseEventData data = new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current);
            transform.GetChild(selectIndex).GetComponent<Button>().OnSubmit(data);
            action.Invoke(transform.GetChild(selectIndex).gameObject);
        }
    }
}
