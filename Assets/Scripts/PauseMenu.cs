using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPasue = false;
    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            if(!GameIsPasue)
            {
                Pause();
            }
            else {
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
        GameIsPasue = false;
    }
    public void Restart() {
        Scene.SceneManager.Instance.TimeRestart();
        Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.GAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }



}
