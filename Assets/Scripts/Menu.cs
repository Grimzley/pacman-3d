using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    private void Start () {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void PlayGame() {
        SceneManager.LoadScene("Game");
    }
    
    public void MainMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
