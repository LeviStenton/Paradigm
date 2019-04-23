using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    PlayerController playerController;

	// Use this for initialization
	void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        StartGame();
	}

    public void RestartGame()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Game");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
    }

    public void EndGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public IEnumerator SpawnNextStage(float time, Transform sceneToUnloadPos, string sceneToUnload, string sceneToLoad)
    {
        sceneToUnloadPos.position = new Vector3(0, -10, 0);
        yield return new WaitForSeconds(time);
        SceneManager.UnloadSceneAsync(sceneToUnload);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        playerController.stageCount += 1;
    }
}
