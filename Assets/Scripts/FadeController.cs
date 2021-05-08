using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するのに必要

public class FadeController : MonoBehaviour {
    [SerializeField]
    float fadeSpeed = 1;
    [SerializeField]
    float targetSpeed = 1;
    [SerializeField]
    float fadeSpeed2 = 3;

    Image fadeImage;
    Image targetImage;
    Image fadeImage2;

    Coroutine coroutine;

    [SerializeField]
    MonoBehaviour nextScript;

    void Start() {
        fadeImage = transform.GetChild(2).GetComponent<Image>();
        targetImage = transform.GetChild(1).GetComponent<Image>();
        fadeImage2 = transform.GetChild(0).GetComponent<Image>();

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
        fadeImage.gameObject.SetActive(false);
        targetImage.gameObject.SetActive(false);
        fadeImage2.gameObject.SetActive(false);
        nextScript.enabled = true;
    }

    IEnumerator Enumerator() {
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(ClearAlpha(fadeImage, fadeSpeed));

        yield return new WaitForSeconds(1);

        yield return StartCoroutine(ClearAlpha(targetImage, targetSpeed));
        yield return StartCoroutine(ClearAlpha(fadeImage2, fadeSpeed2));

        End();
        coroutine = null;
    }


    IEnumerator ClearAlpha(Image image, float speed) {
        Color color = Color.white;

        while (true) {
            color.a -= Time.deltaTime * speed;

            image.color = color;
            yield return null;

            if (color.a < 0) {
                break;
            }
        }
    }
}