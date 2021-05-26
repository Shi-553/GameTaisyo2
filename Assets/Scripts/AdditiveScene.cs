using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditiveScene : MonoBehaviour
{
    private void Awake() {
        var bgm = GameObject.Find("GameBGM");
        if (bgm != null) {
            bgm.GetComponent<AudioSource>().Stop();
        }
        var addCamera = GameObject.Find("AddCamera");
        if (addCamera != null) {
            addCamera.SetActive(false);
            Destroy(addCamera);
        }
#if UNITY_EDITOR
        if (GameObject.FindObjectOfType<AudioListener>()==null) {
            gameObject.AddComponent<AudioListener>();
            gameObject.AddComponent<AudioManager>();
        }
#endif
    }
}
