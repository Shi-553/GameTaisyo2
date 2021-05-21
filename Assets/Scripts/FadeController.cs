using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class FadeController : MonoBehaviour {

    [SerializeField]
    VideoPlayer target;

    Coroutine coroutine;

    [SerializeField]
    MonoBehaviour nextScript;
    [SerializeField]
    AudioSource nextBGM;

    [SerializeField]
    Image image;
    [SerializeField]
    float fadeOutSpeed = 1;
    [SerializeField]
    float fadeStopTime = 0.5f;
    [SerializeField]
    float fadeInSpeed = 1;

    void Start() {
        coroutine =image.StartCoroutine(Enumerator());
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
        while (true) {
            if (image.color.a >= 1) {
                break;
            }
            var c = image.color;
            c.a += Time.unscaledDeltaTime* fadeOutSpeed;
            image.color = c;
            yield return null;
        }
        
        image.color = Color.black;
        End();
        yield return new WaitForSeconds(fadeStopTime);

        while (true) {
            if (image.color.a <= 0) {
                break;
            }
            var c = image.color;
            c.a -= Time.unscaledDeltaTime* fadeInSpeed;
            image.color = c;
            yield return null;
        }

        coroutine = null;
    }
}