using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;




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
        public void ChangeScene(SceneType nSceneType, LoadSceneMode nLoadSceneMode)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)nSceneType, (LoadSceneMode)nLoadSceneMode);
        }
        public void ChangeScene(int nSceneType)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)nSceneType);
        }
    }
}

