using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPasue = false;
    public GameObject ingameMenu;
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
        ingameMenu.SetActive(true);
        GameIsPasue = true;
    }
    public void Resume() {
        Scene.SceneManager.Instance.TimeRestart();
        ingameMenu.SetActive(false);
        GameIsPasue = false;
    }
    public void Restart() {
        Scene.SceneManager.Instance.TimeRestart();
        Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.GAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }



}
