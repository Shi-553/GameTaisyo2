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
        RESULT,
        RESULT2,
    };
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        public void ChangeScene(SceneType nSceneType, LoadSceneMode nLoadSceneMode)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)nSceneType, (LoadSceneMode)nLoadSceneMode);
        }
    }
}

