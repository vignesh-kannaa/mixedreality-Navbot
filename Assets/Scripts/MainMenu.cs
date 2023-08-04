using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void StartNavigation(){
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitApplication(){
        Application.Quit();
    }
}
