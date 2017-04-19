using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Level Manager controls the propagatoin of scenes as well as manages 
 * the concurrent updates in the game play
 */
public class levelManager : MonoBehaviour {
    // Use this for initialization
    public Transform canvas;        // sets the game canvas ( pause menu ) to visible and disabled based on key pressed
    public Transform pauseMenu;     // Handles pause Menu options for the game
    public Transform soundsMenu;    // Will be sound control system. 
    public Transform controlsMenu;  // In case needed will be implemented to let user set custom controls

    SaveGame saveGame;
	
	// Update is called once per frame
	void Update () {
        // if the key down is escape, toggle on/off the pause menu
        if (Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }
    }
    
    /**
     * Resumes game with timeScale functionality set to true to 
     * allow interractive world instead of static
     */
    public void ResumeGame() {
        pauseMenu.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    /*
     * Pause game checks the active Hierachy and sets pause menu to true
     * if it is false and false otherwise. This also saves the game
     * temporariy whilst paused. 
     */
    public void Pause() {
        if (canvas.gameObject.activeInHierarchy == false) {

            if( pauseMenu.gameObject.activeInHierarchy == false) {
                pauseMenu.gameObject.SetActive(true);
                controlsMenu.gameObject.SetActive(false);
                soundsMenu.gameObject.SetActive(false);
            }

            
            canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            // automatically save game and and exit successfully. 
            saveGame = gameObject.GetComponent<SaveGame>();
            saveGame.saveGameSettings(false);

        } else {
            // if the pause game is set to visible, then
            // deactivate it and enable time scaling for game play
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    /**
     *  Method is used to Load or redirect one scene to another. 
     *  Takes a string argument which is nameOfScene and makes it active.
     *  
     *  @Param nameOfScene Scene by name passed to method to be loaded. 
     */
    public void loadLevel(string nameOfScene) {
        // SceneManager is used to complete this task
        SceneManager.LoadScene(nameOfScene);
    }

    /**
     * Simply restart the game as well as its timeScale back to scratch. 
     */
    public void RestartGame() {
        loadLevel("Main");
        Time.timeScale = 1;
    }

    /**
     * Quit game
     */
    public void quitGame() {
        Application.Quit();
    }
}
