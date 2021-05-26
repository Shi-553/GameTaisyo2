using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStage : MonoBehaviour {
    [SerializeField]
    Sprite allClearSprite;

    private void Start() {
        if (StageManager.Instance.IsLastStage) {
            var nextStage = transform.GetChild(0);
            var stageSelect = transform.GetChild(1);

            if (StageManager.Instance.IsAllClear) {
                nextStage.GetComponent<Image>().sprite = allClearSprite;
            }
            else {

                stageSelect.position = nextStage.position;

                nextStage.SetParent(null);
                nextStage.gameObject.SetActive(false);
                Destroy(nextStage.gameObject);

                GetComponent<ButtonManager>().ButtonReset();

            }
        }
    }
    public void Next() {
        if (!StageManager.Instance.IsLastStage) {
            StageManager.Instance.NextStage();
        }

        var nextScene = StageManager.Instance.IsLastStage ? Scene.SceneType.ALLCLEAR : Scene.SceneType.GAME;

        Scene.SceneManager.Instance.ChangeScene(nextScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
