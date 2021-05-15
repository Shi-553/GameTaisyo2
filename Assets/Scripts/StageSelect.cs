using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSelect : MonoBehaviour {

    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Scene.SceneManager.Instance.ChangeScene((int)Scene.SceneType.TITLE);
        }
    }
    public void SetStage(GameObject obj) {
        Scene.SceneManager.stage = int.Parse(obj.name);
    }
}
