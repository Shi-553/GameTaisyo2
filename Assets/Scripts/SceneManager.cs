using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Item;
using System.Linq;

namespace Scene
{
   
    public enum SceneType
    {
        TITLE,
        STAGESELECT,
        GAME,
        GAMECLEAR,
        GAMEOVER,
        ALLCLEAR,
        NONE,
    };
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        SceneType current= SceneType.NONE;
        public SceneType Currnt { get { return current; } }

        private void Start() {

            Application.targetFrameRate = 60;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), LoadSceneMode.Single);

            DontDestroyOnLoad(gameObject);
        }
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene i_loadedScene, LoadSceneMode i_mode) {
            current = (SceneType)i_loadedScene.buildIndex;

        }


        private IEnumerator SceneChangeCorutine(SceneType sceneType) {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
           Transform fadeImage= scene.GetRootGameObjects().FirstOrDefault(g=>g.name=="Canvas").transform.Find("Fade Image");
            FadeCorutine fadeCorutine  = fadeImage.GetComponent<FadeCorutine>();
            yield return StartCoroutine(fadeCorutine.FadeOut());

            TimeRestart();
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneType);

        }

        public void ChangeScene(SceneType nSceneType, LoadSceneMode nLoadSceneMode)
        {
            if (nLoadSceneMode  == LoadSceneMode.Single)
             {
                ChangeScene((int)nSceneType);
                return;
            }
            current = nSceneType;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)nSceneType, (LoadSceneMode)nLoadSceneMode);
        }
        public void ChangeScene(int nSceneType) {
            StartCoroutine(SceneChangeCorutine((SceneType)nSceneType));
        }

        public bool IsTimeStopped { get; private set; }

        float timeScale = 0;
        public void TimeStop() {
            if (IsTimeStopped) {
                return;
            }
            IsTimeStopped = true;
            timeScale = Time.timeScale;

            Time.timeScale = 0;
        }

        public void TimeRestart() {
            if (!IsTimeStopped) {
                return;
            }
            IsTimeStopped = false;
            Time.timeScale = timeScale;
        }

    }
}

