using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : MonoBehaviour {
    [SerializeField]
    float distance = 8;
    [SerializeField]
    GameObject switchGreen;
    [SerializeField]
    GameObject removeBlock;
    [SerializeField]
    GameObject timeStopItem;
    [SerializeField]
    GameObject beltconbyerSwitch;
    [SerializeField]
    Transform leverRotation;

    Transform tutorialRoot;

    CameraManager cameraManager;

    float beforeSpeed = 0;

    Coroutine coroutine;

    [SerializeField]
    AudioClip se;

    void Start() {
        tutorialRoot = GameObject.Find("TutorialRoot").transform;
        cameraManager = Camera.main.GetComponent<CameraManager>();

    }

    void Update() {
        if (Scene.SceneManager.Instance.IsTimeStopped || cameraManager.Speed == 0) {
            return;
        }
        if (coroutine != null) {
            return;
        }

        var playerPos = Player.PlayerCore.Instance.transform.position;
        int mask = LayerMask.GetMask(new string[] { "Mebiusu" });

        for (int i = 0; i < tutorialRoot.childCount; i++) {
            var child = tutorialRoot.GetChild(i);
            if (!child.gameObject.activeSelf || child.GetChild(0).gameObject.activeSelf) {
                continue;
            }
            var ray = new Ray(child.position, playerPos - child.position);

            if (Physics.Raycast(ray, 10, mask)) {
                continue;
            }
            if (i == 4) {
                if (Vector3.Distance(child.position, cameraManager.MebiusuPoint) > distance-3) {
                    continue;
                }
            }
            else {
                if (Vector3.Distance(child.position, cameraManager.MebiusuPoint) > distance) {
                    continue;
                }
            }

            coroutine = StartCoroutine(WaitAction(i));
            return;
        }
    }

    IEnumerator WaitAction(int i) {
        var child = tutorialRoot.GetChild(i);
        beforeSpeed = cameraManager.Speed;
        cameraManager.Speed *= 0.1f;

        switch (i) {
            case 0:
                yield return new WaitUntil(() => !switchGreen.activeSelf);
                break;

            case 1:
                yield return new WaitUntil(() => !removeBlock.activeSelf);
                break;

            case 2:
                timeStopItem.SetActive(true);
                yield return new WaitUntil(() => cameraManager.IsStopped);
                break;

            case 3:
                yield return new WaitUntil(() => !beltconbyerSwitch.activeSelf);
                break;

            case 4:
                var leverChildRotate = leverRotation.rotation;

                yield return new WaitUntil(() => leverChildRotate!= leverRotation.rotation);
                break;


            default:
                break;
        }
        AudioManager.Instance.Play(se);

        child.GetChild(0).gameObject.SetActive(true);
        cameraManager.Speed = beforeSpeed;
        beforeSpeed = 0;
        yield return new WaitForSeconds(0.1f);
        coroutine = null;
    }
}
