using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public enum SceneType
{
    TITLE,
    GAME,
    RESULT,
};

namespace Scene
{

    public class SceneManager : MonoBehaviour
    {
        public void ChangeScene(int SceneType, int LoadSceneMode)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneType, (LoadSceneMode)LoadSceneMode);
        }
    }
}

