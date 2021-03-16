using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour, IOperatedPlayerObject
{

    void IOperatedPlayerObject.Hit(){
        Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.RESULT, LoadSceneMode.Single);
    }

}