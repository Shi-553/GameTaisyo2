using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{

    public AudioClip clip;
    public AudioSource source;
    
    public void click()
    {
        source.PlayOneShot(clip);
    }

}
