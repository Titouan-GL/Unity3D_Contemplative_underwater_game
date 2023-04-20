using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Slider loadingBar;

    public void PlayGame(){
        LoadLevel("Game");
    }
    
    public void LoadLevel(string sceneName){
        StartCoroutine(LoadAsyncronously(sceneName));
    }

    IEnumerator LoadAsyncronously (string sceneName){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingBar.gameObject.SetActive(true);

        while (!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress/0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }


    public void QuitGame(){
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
