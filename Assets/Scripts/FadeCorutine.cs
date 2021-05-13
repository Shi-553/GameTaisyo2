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

    private void Awake() {
        coroutine=StartCoroutine(FadeIn());
    }
    public IEnumerator FadeOut()
    {
        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        Image image = GetComponent<Image>();
        image.enabled  =  true;
        Color color = image.color;
        color.a = 0;
        image.color = color;

        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (color.a > 1) {
                break;
            }

            color.a += Time.unscaledDeltaTime/fadeoutTime;
            image.color = color;

            yield return new WaitForEndOfFrame();
        }
        image.enabled = false;
        coroutine = null;
    }
    public IEnumerator FadeIn() {

        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        Image image = GetComponent<Image>();
        image.enabled  =  true;
        Color color = image.color;
        color.a = 1;
        image.color = color;

        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (color.a<=0) {
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
