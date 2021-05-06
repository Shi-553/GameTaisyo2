using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonoBehaviour<AudioManager> {
    AudioSource[] audioSources;

    void Start() {
        audioSources = GetComponents<AudioSource>();
    }
    public int Play(AudioClip seclip) {
        for (int i = 0; i < audioSources.Length; i++) {
            if (!audioSources[i].isPlaying) {
                audioSources[i].PlayOneShot(seclip);
                return i;

            }
        }
        return -1;

    }
    public void Stop(int id) {
         audioSources[id].Stop();

    }



}
