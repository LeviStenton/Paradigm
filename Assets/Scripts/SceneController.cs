using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour {

    PlayerController playerController;
    public Animator sceneContAnim;
        
    public Image fadeToBlack;

    public bool endGameSecretBool = false;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        sceneContAnim = GetComponent<Animator>();
	}

    public void RestartGame()
    {
        StartCoroutine(FadeToBlackMM(3, "Main Menu"));
    }

    public void PlayGame()
    {
        StartCoroutine(FadeToBlackStart(3, "Game"));        
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
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sceneToUnloadPos.position = new Vector3(0, -10, 0);
        yield return new WaitForSeconds(time);
        SceneManager.UnloadSceneAsync(sceneToUnload);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        playerController.stageCount += 1;
    }

    public IEnumerator FadeToBlackStart(float time, string sceneToLoad)
    {    
        sceneContAnim.Play("FadeToBlack");        
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneToLoad);
        StartGame();        
        sceneContAnim.Play("FadeFromBlack");
        GameObject[] sceneConts = GameObject.FindGameObjectsWithTag("SceneController");
        foreach (GameObject sceneCont in sceneConts)
        {
            if (sceneCont != gameObject)
                Destroy(sceneCont);
        }
    }

    public IEnumerator FadeToBlackMM(float time, string sceneToLoad)
    {
        Debug.Log("Hit End");
        sceneContAnim.Play("FadeToBlack");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneToLoad);
        sceneContAnim.Play("FadeFromBlack");
        GameObject[] sceneConts = GameObject.FindGameObjectsWithTag("SceneController");
        foreach (GameObject sceneCont in sceneConts)
        {
            if (sceneCont != gameObject)
                Destroy(sceneCont);
        }
    }

    public IEnumerator FadeToBlackEnd(float time, string sceneToLoad)
    {
        endGameSecretBool = true;
        sceneContAnim.Play("FadeToBlack");        
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneToLoad);
        sceneContAnim.Play("FadeFromBlack");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;       
    }    

    #region Pref Setters

    public void SetPref(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public void SetPref(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public void SetPref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public void SetPref(string key, bool value)
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
    }

    public bool GetBoolPref(string key, bool defaultValue = true)
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue)));
    }
    #endregion

}
