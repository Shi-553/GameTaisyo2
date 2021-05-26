using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SerialNumberPng : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;
    public void Play() {
        StartCoroutine(Anime());
    }
    IEnumerator Anime() {
        var image = GetComponent<Image>();
        for (int i = 0; i < sprites.Count; i++) {
            image.sprite = sprites[i];
            yield return null;
        }
    }
}
