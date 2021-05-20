using Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCorutine : MonoBehaviour
{
    float fadeoutTime = 2;
    float fadeinTime = 1.5f;

    Coroutine coroutine;
    bool isFadein = true;

    public bool CanStart { get { return coroutine==null||isFadein; } }

    private void Awake() {
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeOut() {
        if (!CanStart) {
            yield break;
        }
        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        isFadein = false;
        coroutine = StartCoroutine(FO());
        yield return coroutine;
    }
    public IEnumerator FO() {

        Image image = GetComponent<Image>();
        image.enabled = true;
        Color color = image.color;
        color.a = 0;
        image.color = color;

        yield return new WaitForEndOfFrame();
        while (true) {
            if (color.a > 1) {
                break;
            }

            color.a += Time.unscaledDeltaTime / fadeoutTime;
            image.color = color;

            yield return new WaitForEndOfFrame();
        }
        image.enabled = false;
        coroutine = null;
    }
    public IEnumerator FadeIn() {
        if (!CanStart) {
            yield break;
        }

        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(FI());
        isFadein = true;
        yield return coroutine;
    }
    
    public IEnumerator FI() {

        Image image = GetComponent<Image>();
        image.enabled = true;
        Color color = image.color;
        color.a = 1;
        image.color = color;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        while (true) {
            if (color.a <= 0) {
                break;
            }

            color.a -= Time.unscaledDeltaTime / fadeinTime;
            image.color = color;

            yield return new WaitForEndOfFrame();
        }
        image.enabled = false;
        coroutine = null;
    }


}
