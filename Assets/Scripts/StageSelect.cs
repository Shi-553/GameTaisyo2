using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour {
    [SerializeField]

    private void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            var c = transform.GetChild(i);
            var stage = StageManager.Instance.GetData(int.Parse(c.name));
            if (stage == null) {
                Destroy(c.gameObject,0.1f);
                continue;
            }

            switch (stage.status) {
                case StageStatus.LOCK:
                    c.GetComponent<Image>().color = Color.gray;
                    break;
                case StageStatus.UNLOCK:
                    break;
                case StageStatus.CLEAR:
                    c.GetComponent<Image>().color = Color.yellow;
                    break;
                case StageStatus.NO_DAMAGE:
                    c.GetComponent<Image>().color = new Color(1,0.5f,0.5f);
                    break;
                case StageStatus.PURE_NO_DAMAGE:
                    c.GetComponent<Image>().color = Color.red;
                    break;
                default:
                    break;
            }
        }
    }
    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Scene.SceneManager.Instance.ChangeScene((int)Scene.SceneType.TITLE);
        }
    }
    public void SetStage(GameObject obj) {
        StageManager.Instance.SetStage( int.Parse(obj.name));
    }
}
