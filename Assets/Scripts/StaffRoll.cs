using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StaffRoll : MonoBehaviour
{
    [SerializeField]
    GameObject endB;
    bool isEnd = false;

    PlayableDirector playableDirector;
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update() {
        if (isEnd) {
            return;
        }
        if (playableDirector.state == PlayState.Paused) {
            Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.TITLE, UnityEngine.SceneManagement.LoadSceneMode.Single);
            isEnd = true;
            return;
        }
        if (Input.GetButtonDown("Cancel")&& endB.activeSelf) {
            Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.TITLE, UnityEngine.SceneManagement.LoadSceneMode.Single);
            isEnd = true;
        }
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")) {
            StartCoroutine(Active());
        }
    }
    IEnumerator Active() {
        endB.SetActive(true);
        yield return new WaitForSeconds(3);
        endB.SetActive(false);
    }
}
