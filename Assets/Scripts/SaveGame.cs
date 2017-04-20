using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour {

    public Transform play;
    private levelManager manager;

    void Awake() {
        play.position = new Vector2 (PlayerPrefs.GetFloat("X"), PlayerPrefs.GetFloat("y"));
    }

    public void saveGameSettings(bool quit) {
        /*
         * This is going to save a file that can be reloaded 
         * This file is practically secured and unaccessible
         */
        PlayerPrefs.SetFloat("x", play.position.x);
        PlayerPrefs.SetFloat("y", play.position.y);

        if( quit) {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main");
            manager.quitGame();
        }
    }
	
}
