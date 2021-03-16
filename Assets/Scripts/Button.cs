﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    [SerializeField] Scene.SceneType sceneType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
     void OnClickButton()
    {
        Scene.SceneManager.Instance.ChangeScene(sceneType, LoadSceneMode.Single);
    }
}
