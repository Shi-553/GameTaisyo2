using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField]
    Transform selectImage;

    [SerializeField]
    Vector3 selectScale;
    [SerializeField]
    Color selectColor;

    int selectIndex = 0;
    bool pressed = false;

    [SerializeField] bool swapVH = false;

    [SerializeField] bool reverseV = false;
    [SerializeField] bool reverseH = false;

    [SerializeField] int columnMax = 0;
    
    [SerializeField]
    AudioClip se;

    private void Start() {
        var selected = transform.GetChild(selectIndex);

        if (selectImage != null) {
            selectImage.position = selected.position;
        }
        selected.localScale += selectScale;
        selected.GetComponent<Image>().color = selectColor;
    }


    void Update() {


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
                transform.GetChild(selectIndex).localScale -= selectScale;
                transform.GetChild(selectIndex).GetComponent<Image>().color = Color.white;
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
                selected.GetComponent<Image>().color = selectColor;
            }
        }

        if (v == 0 && h == 0) {
            pressed = false;
        }

        if (Input.GetButtonDown("Submit")) {
            UnityEngine.EventSystems.BaseEventData data = new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current);
            transform.GetChild(selectIndex).GetComponent<Button>().OnSubmit(data);
        }
    }
}
