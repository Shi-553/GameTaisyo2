using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public void Next() {
        var canNextStage=StageManager.Instance.NextStage();
        var nextScene = canNextStage ? Scene.SceneType.GAME : Scene.SceneType.ALLCLEAR;
        Scene.SceneManager.Instance.ChangeScene(nextScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
