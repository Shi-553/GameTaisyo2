using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    AudioClip se;
    [SerializeField]
    AudioClip ok;
    [SerializeField]
    AudioClip cancel;

    public bool GameIsPasue = false;
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {      
            if (!GameIsPasue) {
                if (Scene.SceneManager.Instance.IsTimeStopped) {
                    return;
                }
                AudioManager.Instance.Play(se);
                Pause();
            }
            else {
                AudioManager.Instance.Play(cancel);
                Resume();
            }
        }
    }
    public void Pause() {
        Scene.SceneManager.Instance.TimeStop();
        transform.GetChild(0).gameObject.SetActive(true);
        GameIsPasue = true;

    }
    public void Resume() {
        Scene.SceneManager.Instance.TimeRestart();
        transform.GetChild(0).gameObject.SetActive(false);
        //AudioManager.Instance.Play(ok);
        GameIsPasue = false;
    }
    public void Restart() {
        Scene.SceneManager.Instance.TimeRestart();
        Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.GAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
        //AudioManager.Instance.Play(ok);
    }



}
