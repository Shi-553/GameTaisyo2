using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Item;

namespace Scene
{
   
    public enum SceneType
    {
        TITLE,
        STAGESELECT,
        GAME,
        GAMECLEAR,
        GAMEOVER,
    };
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        private IEnumerator SceneChangeCorutine(SceneType sceneType)
        {
            GameObject fadeImage= GameObject.Find("Fade Image");
            FadeCorutine fadeCorutine  = fadeImage.GetComponent<FadeCorutine>();
            yield return StartCoroutine(fadeCorutine.Fade(sceneType));

            UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneType);

        }

        public void ChangeScene(SceneType nSceneType, LoadSceneMode nLoadSceneMode)
        {
            if (nLoadSceneMode  == LoadSceneMode.Single)
             {
                ChangeScene((int)nSceneType);
                return;
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)nSceneType, (LoadSceneMode)nLoadSceneMode);
        }
        public void ChangeScene(int nSceneType) {
            StartCoroutine(SceneChangeCorutine((SceneType)nSceneType));
        }

        public bool IsTimeStopped { get; private set; }

        public void TimeStop() {
            IsTimeStopped = true;

            Time.timeScale = 0;
            var a = FindObjectsOfType<Component>();
            foreach (var c in a) {
                var timeStopable = c as TimeStopable;

                if (timeStopable != null) {
                    timeStopable.TimeStopped();
                }
            }

        }

        public void TimeRestart() {
            IsTimeStopped = false;
            Time.timeScale = 1f;


            var a = FindObjectsOfType<Component>();
            foreach (var c in a) {
                var timeStopable = c as TimeStopable;

                if (timeStopable != null) {
                    timeStopable.TimeReStarted();
                }
            }
        }

    }
}

