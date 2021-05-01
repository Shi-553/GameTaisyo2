using Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCorutine : MonoBehaviour
{
    [SerializeField]
    int fadetime = 100;
    public IEnumerator Fade(SceneType sceneType)
    {
        
        Image image = GetComponent<Image>();
        image.enabled  =  true;
        Color color = image.color;
        color.a = 0;
        image.color = color;

        for (int i = 0; i < fadetime; i++)
        {
            
            color = image.color;
            color.a += 1.0f / fadetime;
            image.color = color;

            yield return new WaitForEndOfFrame();
        }
        image.enabled = false;
    }


}
