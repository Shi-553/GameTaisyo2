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
    GameObject timeStopItemHideBlock;
    [SerializeField]
    GameObject beltconbyerSwitch;
    [SerializeField]
    Transform leverRotation;
    Quaternion leverChildRotate;

    Transform tutorialRoot;

    CameraManager cameraManager;

    float beforeSpeed = 0;
    bool isSpeedDown = false;

    Coroutine coroutine;

    [SerializeField]
    AudioClip se;

    int currentIndex = 0;

    void Start() {
        tutorialRoot = GameObject.Find("TutorialRoot").transform;
        cameraManager = Camera.main.GetComponent<CameraManager>();
        leverChildRotate = leverRotation.rotation;

    }

    void Update() {
        if (Scene.SceneManager.Instance.IsTimeStopped || cameraManager.Speed == 0) {
            return;
        }


        if (currentIndex >= tutorialRoot.childCount) {
            return;
        }

        var child = tutorialRoot.GetChild(currentIndex);

        //そのアクションクリア済みならスルー
        if (child.GetChild(0).gameObject.activeSelf) {
            return;
        }

        if (coroutine == null) {
            //ある程度近づいたら吹き出し出してアクション待機
            if (!child.gameObject.activeSelf) {
                if (((currentIndex == 4 || currentIndex == 2) && Vector3.Distance(child.position, cameraManager.MebiusuPoint) < distance - 3 + 3) ||
                    ((currentIndex != 4 && currentIndex != 2) && Vector3.Distance(child.position, cameraManager.MebiusuPoint) < distance + 3)) {
                    child.gameObject.SetActive(true);

                    coroutine = StartCoroutine(WaitAction());
                }
            }
        }
        else {
            //すごく近づいたら遅くする
            if (!isSpeedDown &&
                (((currentIndex == 4 || currentIndex == 2) && Vector3.Distance(child.position, cameraManager.MebiusuPoint) < distance - 3) ||
                ((currentIndex != 4 && currentIndex != 2) && Vector3.Distance(child.position, cameraManager.MebiusuPoint) < distance))) {
                ChangeSpeed();
            }
        }
    }
    void ChangeSpeed() {
        isSpeedDown = true;
        beforeSpeed = cameraManager.StartSpeed;
        cameraManager.Speed *= 0.2f;
    }

    IEnumerator WaitAction() {
        var child = tutorialRoot.GetChild(currentIndex);
        child.gameObject.SetActive(true);

        switch (currentIndex) {
            case 0:
                yield return new WaitUntil(() => !switchGreen.activeSelf);
                break;

            case 1:
                yield return new WaitUntil(() => !removeBlock.activeSelf);
                break;

            case 2:
                timeStopItemHideBlock.SetActive(false);
                timeStopItem.GetComponent<Collider>().isTrigger = true;
                yield return new WaitUntil(() => cameraManager.IsStopped);
                break;

            case 3:
                yield return new WaitUntil(() => !beltconbyerSwitch.activeSelf);
                break;

            case 4:

                yield return new WaitUntil(() => leverChildRotate != leverRotation.rotation);
                break;


            default:
                break;
        }
        AudioManager.Instance.Play(se);

        child.GetChild(0).gameObject.SetActive(true);
        if (isSpeedDown) {
            cameraManager.Speed = beforeSpeed;
            beforeSpeed = 0;
            isSpeedDown = false;
        }
        coroutine = null;
        currentIndex++;
        yield return new WaitForSeconds(3);

        child.gameObject.SetActive(false);
    }
}
