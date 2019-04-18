using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartGame();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RestartGame();
    }

    void RestartGame()
    {
        if (Input.GetButtonDown("Escape"))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void NextStage(string sceneToUnload, string sceneToLoad)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }
}
