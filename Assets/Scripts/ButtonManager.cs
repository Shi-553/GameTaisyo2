using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    [SerializeField]
    Transform selectImage;
    int selectIndex = 0;
    bool pressed = false;

    [SerializeField] bool swapVH = false;

    [SerializeField] bool reverseV = false;
    [SerializeField] bool reverseH = false;

    [SerializeField] int columnMax = 0;

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
        }
        if (v == 0 && h == 0) {
            pressed = false;
        }

        if (selectIndex < 0) {
            selectIndex = (selectIndex+ transform.childCount) % transform.childCount;
        }
        if (selectIndex > transform.childCount - 1) {
            selectIndex = selectIndex % transform.childCount;
        }
        selectImage.position = transform.GetChild(selectIndex).position;


        if (Input.GetButtonDown("Submit")) {
            UnityEngine.EventSystems.BaseEventData data = new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current);
            transform.GetChild(selectIndex).GetComponent<Button>().OnSubmit(data);
        }
    }
}
