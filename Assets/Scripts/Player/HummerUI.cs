using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HummerUI : MonoBehaviour {
    Slider slider;
    private void Start() {
        slider = transform.Find("Slider").GetComponent<Slider>();

    }
    public void SetSlider(float value) {
        slider.value = value;
    }
}
