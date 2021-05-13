using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject nextButton;
    [SerializeField]
    AudioClip clip;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Submit")) {
            OnClick();
        }
    }

    public void OnClick() {
        nextButton.SetActive(true);
        this.gameObject.SetActive(false);
        AudioManager.Instance.Play(clip);

    }
}
