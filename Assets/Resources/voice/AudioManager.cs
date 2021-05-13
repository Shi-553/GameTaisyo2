using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonoBehaviour<AudioManager> {
    AudioSource[] audioSources;
    int[] exclusionIds;

    int id = 0;
    int NewExclusionId {
        get {
            id++;
            return id;
        }
    }

    void Start() {
        audioSources = GetComponents<AudioSource>();
        exclusionIds = new int[audioSources.Length];
    }
    public int Play(AudioClip seclip, bool isExclusion = false) {
        if (isExclusion) {
            for (int i = 1; i < audioSources.Length; i++) {
                if (!audioSources[i].isPlaying) {
                    audioSources[i].PlayOneShot(seclip);
                    int id = NewExclusionId;
                    exclusionIds[i] = id;

                    return id;

                }
            }
        }
        else {
            audioSources[0].PlayOneShot(seclip);
            return -1;
        }
        return -1;

    }
    public void Stop(int id) {
        for (int i = 0; i < exclusionIds.Length; i++) {
            if (exclusionIds[i] == id) {
                audioSources[i].Stop();
            }
        }

    }



}
