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
        public void ChangeScene(SceneType nSceneType, LoadSceneMode nLoadSceneMode) {
            Time.timeScale = 1.0f;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)nSceneType, (LoadSceneMode)nLoadSceneMode);
        }
        public void ChangeScene(int nSceneType) {
            Time.timeScale = 1.0f;
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)nSceneType);
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

