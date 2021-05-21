using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FadeController : MonoBehaviour {

    [SerializeField]
    VideoPlayer target;

    Coroutine coroutine;

    [SerializeField]
    MonoBehaviour nextScript;
    [SerializeField]
    AudioSource nextBGM;

    void Start() {
        coroutine = StartCoroutine(Enumerator());
    }

    void Update() {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Pause")) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                coroutine = null;
                End();
            }
        }
    }
    void End() {
        nextBGM.Play();
        gameObject.SetActive(false);
        nextScript.enabled = true;
    }

    IEnumerator Enumerator() {
        yield return new WaitUntil(() => target.isPlaying);
        yield return new WaitWhile(() => target.isPlaying);

        End();
        coroutine = null;
    }
}