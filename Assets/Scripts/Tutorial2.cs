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
    Quaternion leverChildRotate;

    Transform tutorialRoot;

    CameraManager cameraManager;

    float beforeSpeed = 0;

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
        if (coroutine != null) {
            return;
        }


        var child = tutorialRoot.GetChild(currentIndex);
        if (child.GetChild(0).gameObject.activeSelf) {
            return;
        }

        if (currentIndex == 4) {
            if (Vector3.Distance(child.position, cameraManager.MebiusuPoint) > distance - 3) {
                return;
            }
        }
       else if (currentIndex == 2) {
            if (Vector3.Distance(child.position, cameraManager.MebiusuPoint) > distance - 3) {
                return;
            }
        }
        else {
            if (Vector3.Distance(child.position, cameraManager.MebiusuPoint) > distance) {
                return;
            }
        }

        coroutine = StartCoroutine(WaitAction());
    }

    IEnumerator WaitAction() {
        var child = tutorialRoot.GetChild(currentIndex);
        child.gameObject.SetActive(true);
        beforeSpeed = cameraManager.Speed;
        cameraManager.Speed *= 0.1f;

        switch (currentIndex) {
            case 0:
                yield return new WaitUntil(() => !switchGreen.activeSelf);
                break;

            case 1:
                yield return new WaitUntil(() => !removeBlock.activeSelf);
                break;

            case 2:
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
        cameraManager.Speed = beforeSpeed;
        beforeSpeed = 0;
        yield return new WaitForSeconds(0.1f);
        coroutine = null;
        currentIndex++;
        yield return new WaitForSeconds(3);

        child.gameObject.SetActive(false);
    }
}
