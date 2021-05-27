using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : MonoBehaviour {
    [SerializeField]
    float distance = 3;
    [SerializeField]
    GameObject heart;
    [SerializeField]
    List<GameObject> breaks;
    [SerializeField]
    GameObject repair;
    [SerializeField]
    Rigidbody hitBlock;
    [SerializeField]
    GameObject bomb;
    [SerializeField]
    List<GameObject> bombBleaks;
    [SerializeField]
    GameObject goalSwitch;

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
                if ((currentIndex == 7 && Vector3.Distance(bombBleaks[0].transform.position, cameraManager.MebiusuPoint) < distance - 1 + 3) ||
                    (currentIndex != 7 && Vector3.Distance(child.position, cameraManager.MebiusuPoint) < distance + 3)) {
                    child.gameObject.SetActive(true);

                    coroutine = StartCoroutine(WaitAction());
                }
            }
        }
        else {
            //すごく近づいたら遅くする
            if (!isSpeedDown &&
                ((currentIndex == 7 && Vector3.Distance(bombBleaks[0].transform.position, cameraManager.MebiusuPoint) < distance - 1) ||
                (currentIndex != 7 && Vector3.Distance(child.position, cameraManager.MebiusuPoint) < distance))) {
                ChangeSpeed();
            }
        }

    }
    void ChangeSpeed() {
        isSpeedDown = true;
        beforeSpeed = cameraManager.Speed;

        switch (currentIndex) {
            case 1:
            case 6:
                break;


            default:
                cameraManager.Speed *= 0.2f;
                break;
        }
    }
    IEnumerator WaitAction() {
        var child = tutorialRoot.GetChild(currentIndex);


        switch (currentIndex) {
            case 0:
                yield return new WaitUntil(() => {
                    var h = Input.GetAxis("Horizontal");
                    var v = Input.GetAxis("Vertical");

                    return h != 0 || v != 0;
                });
                yield return new WaitForSeconds(1);
                break;

            case 1:
                yield return new WaitUntil(() => Vector3.Distance(child.position, cameraManager.MebiusuPoint) < distance);

                yield return new WaitForSeconds(1);

                yield return new WaitUntil(() => Vector3.Distance(child.position, cameraManager.MebiusuPoint) > distance);
                break;

            case 2:
                heart.GetComponent<Collider>().isTrigger = true;
                yield return new WaitUntil(() => !heart.activeSelf);
                break;

            case 3:
                yield return new WaitUntil(() => {
                    foreach (var block in breaks) {
                        if (!block.activeSelf) {
                            return true;
                        }
                    }
                    return false;
                });
                break;

            case 4:
                var hummerHp = Player.PlayerHummer.Instance.Hp;
                repair.GetComponent<Collider>().isTrigger = true;

                yield return new WaitUntil(() => {
                    if (Player.PlayerHummer.Instance.Hp > hummerHp) {
                        return true;
                    }
                    hummerHp = Player.PlayerHummer.Instance.Hp;
                    return false;
                });
                break;

            case 5:
                yield return new WaitUntil(() => {
                    return hitBlock.velocity.magnitude > 0.1f;
                });
                break;

            case 6:
                yield return new WaitForSeconds(5);
                break;

            case 7:
                bomb.GetComponent<Collider>().isTrigger = true;
                yield return new WaitUntil(() => {
                    foreach (var block in bombBleaks) {
                        if (!block.activeSelf) {
                            return true;
                        }
                    }
                    return false;
                });
                break;

            case 8:
                yield return new WaitUntil(() => !goalSwitch.activeSelf);

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


#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (!UnityEditor.EditorApplication.isPlaying) {
            var t = GameObject.Find("TutorialRoot")?.transform;
            if (t == null) {
                return;
            }
            for (int i = 0; i < t.childCount; i++) {
                var child = t.GetChild(i);
                Gizmos.DrawWireSphere(child.position, distance);
            }
        }
    }
#endif
}
