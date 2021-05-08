using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject StartButton, EndButton;
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Submit")) {
            OnClick();
        }
    }

    public void OnClick() {
        StartButton.SetActive(true);
        EndButton.SetActive(true);
        this.gameObject.SetActive(false);


    }
}
