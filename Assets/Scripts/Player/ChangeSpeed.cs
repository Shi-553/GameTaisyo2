using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeSpeed : MonoBehaviour {

    [SerializeField]
    AudioClip speedUpSE;
    [SerializeField]
    AudioClip speedDownSE;

    IntObjectUI speedUI;
    float scale;

    CameraManager cameraManager;
    bool isUp = true;

    Coroutine animeCo;

    IntObjectUI SpeedUI {
        get {
            if (speedUI == null) {
                speedUI = GameObject.Find("Canvas/SpeedScaleRoot").GetComponent<IntObjectUI>();
            }
            return speedUI;
        }
    }
    private void Start() {
        cameraManager = Camera.main.GetComponent<CameraManager>();
        scale = cameraManager.Speed;
    }


    private void Update() {
        if (Scene.SceneManager.Instance.IsTimeStopped || cameraManager.Speed == 0) {
            return;
        }
        if (!Mathf.Approximately(cameraManager.Speed, scale)) {
            return;
        }

        if (!Input.GetButtonDown("SpeedChange")) {
            return;
        }

        TimeChange(isUp);
        isUp = !isUp;
    }

    void TimeChange(bool isSpeedUp) {
        scale = cameraManager.Speed;

        if (isSpeedUp) {
            scale *= 2.0f;
            AudioManager.Instance.Play(speedUpSE);
            if (animeCo == null) {
                animeCo = StartCoroutine(SpeedUpAnime());
            }
        }
        else {
            scale /= 2.0f;
            AudioManager.Instance.Play(speedDownSE);
            if (animeCo != null) {
                StopCoroutine(animeCo);
                SpeedUI.Clear();
                animeCo = null;
            }
        }


        cameraManager.Speed = scale;

    }
    IEnumerator SpeedUpAnime() {
        SpeedUI.SetMax(3, 0);
        while (true) {
            SpeedUI.Add();
            yield return new WaitForSeconds(1);
            SpeedUI.Add();
            yield return new WaitForSeconds(1);
            SpeedUI.Add();
            yield return new WaitForSeconds(1);
            SpeedUI.Remove(3);
            yield return new WaitForSeconds(1);
        }
    }
}
