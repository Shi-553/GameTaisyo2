using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour {
    [SerializeField]

    private void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            var childRoot = transform.GetChild(i);
            var image = childRoot.GetChild(1);
            var stage = StageManager.Instance.GetData(int.Parse(childRoot.name));
            if (stage == null) {
                Destroy(childRoot.gameObject,0.1f);
                continue;
            }

            switch (stage.status) {
                case StageStatus.LOCK:
                    image.GetComponent<Image>().color = Color.gray;
                    break;
                case StageStatus.UNLOCK:
                    break;
                case StageStatus.CLEAR:
                    image.GetComponent<Image>().color = new Color(1, 0.95f, 0.42f);
                    break;
                case StageStatus.NO_DAMAGE:
                    image.GetComponent<Image>().color = new Color(1,0.64f,0.72f);
                    break;
                case StageStatus.PURE_NO_DAMAGE:
                    image.GetComponent<Image>().color = new Color(1, 0.64f, 0.72f);
                    break;
                default:
                    break;
            }
        }
    }
    private void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Scene.SceneManager.Instance.ChangeScene((int)Scene.SceneType.TITLE);
            GetComponent<ButtonManager>().ButtonEnd();
        }
    }
    public void SetStage(GameObject obj) {
        StageManager.Instance.SetStage( int.Parse(obj.name));
    }
}
