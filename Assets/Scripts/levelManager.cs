using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour {
    // Use this for initialization
    private bool isEnable;
	void Start () {
        isEnable = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		makePauseSceneActive();

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
     * Brings up the In Game Paused Menu when Esc Key is pressed  
     */
	public void makePauseSceneActive( ) {
		if (Input.GetKeyDown (KeyCode.Escape) && !isEnable) {
            Debug.Log("0");
            loadLevel ("InGamePausedMenu");
            isEnable = true;
        } else if (Input.GetKeyDown (KeyCode.Escape) && isEnable) {
            Debug.Log("1");
            loadLevel("Game");
            isEnable = false;
        }

    }

    public void quitGame() {
        Application.Quit();
    }
}
