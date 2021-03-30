using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour, IOperatedPlayerObject
{
    public void Show() {
        gameObject.SetActive(true);
    }
    void IOperatedPlayerObject.Hit(){
        Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.RESULT, LoadSceneMode.Single);
    }

}