﻿using System;
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
        NONE,
    };
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        [SerializeField]
        int debugStage = 1;
        public static int stage = 0;
        SceneType current= SceneType.NONE;

        private void Start() {

            Application.targetFrameRate = 60;
            current = (SceneType)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            if (current == SceneType.GAME) {
                var s = (stage == 0) ? debugStage : stage;
                var stagePrefab = Resources.Load<GameObject>("Stage/" + s.ToString());
                var stageInstance=Instantiate<GameObject>(stagePrefab);

                stageInstance.transform.SetParent(GameObject.Find("stage").transform);
            }
        }
        private IEnumerator SceneChangeCorutine(SceneType sceneType)
        {
            GameObject fadeImage= GameObject.Find("Fade Image");
            FadeCorutine fadeCorutine  = fadeImage.GetComponent<FadeCorutine>();
            yield return StartCoroutine(fadeCorutine.Fade(sceneType));

            Time.timeScale = 1.0f;
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

