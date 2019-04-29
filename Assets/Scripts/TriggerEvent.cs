using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerEvent : MonoBehaviour {

    PlayerSSUIController playerSSUI;

    [Header("Arrays")]
    public GameObject[] screens;
    public GameObject[] screenLight;
    public GameObject[] storyTexts;
    int currentIndex = 0;

    [Header("Strings")]
    public string sceneToUnload;
    public string sceneToLoad;
    
    PlayerController playerController;
    SceneController sceneCont;

    BoxCollider col;

    [Header("Timers")]
    public float staticEffectTime;
    public float timeUntilNextScene;

    [Header("Vectors")]
    public Vector3 endGameStageOffSet;

    //GRAB BOX COLLIDER ON SCREEN TO DEACTIVATE DURING TRANSITION SO INDEX++ DOESN"T INCREASE DURING TRANSITIONS
    public void Start()
    {
        col = this.gameObject.GetComponent<BoxCollider>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sceneCont = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        playerSSUI = GameObject.FindGameObjectWithTag("SSUI").GetComponent<PlayerSSUIController>();
    }

    void FixedUpdate()
    {
        if (playerSSUI.isPaused)
        {
            stage6PlayerFollow();
        }
    }

    void stage6PlayerFollow()
    {
        if (playerController.stageCount >= 6f)
        {
            GameObject.FindGameObjectWithTag("Final Stage").transform.position = new Vector3(0, 0, playerController.transform.position.z + endGameStageOffSet.z);
}
    }

    //THE FUNCTION TO BE CALLED BY THE PLAYER'S RAYCAST
    public void TriggeredEvent()
    {
        //The '- 1' is to compensate for 'Length' not starting at 0, but 1, while the index always starts at 0.
        if(currentIndex < (storyTexts.Length - 1))
        {
            storyTexts[currentIndex].SetActive(false);
            StartCoroutine(StaticEffect(staticEffectTime));
            currentIndex++;
            storyTexts[currentIndex].SetActive(true);
        }

        else if(currentIndex >= (storyTexts.Length - 1))
        {
            currentIndex = storyTexts.Length;
            StartCoroutine(StaticEffectEnd(staticEffectTime, this.transform, sceneToUnload, sceneToLoad));
        }
    }

    //NEW TEXT, PLAY STATIC SCREEN TO TRANSITION BETWEEN TEXTS
    IEnumerator StaticEffect(float time)
    {
        col.enabled = false;
        screens[0].SetActive(false);
        screens[1].SetActive(true);
        screenLight[0].SetActive(false);
        screenLight[1].SetActive(true);
        yield return new WaitForSeconds(time);
        screens[0].SetActive(true);
        screens[1].SetActive(false);
        screenLight[0].SetActive(true);
        screenLight[1].SetActive(false);
        col.enabled = true;
    }

    //END SCENE - TRANSITION TO NEW SCENE
    IEnumerator StaticEffectEnd(float time, Transform stup, string stu, string stl)
    {
        col.enabled = false;
        screens[0].SetActive(false);
        screenLight[0].SetActive(false);
        playerController.playerScreenStatic.SetActive(true);
        yield return new WaitForSeconds(time);
        ResetPlayerPos();        
        playerController.playerScreenStatic.SetActive(false);   
        if(playerController.stageCount == 5)
        {
            StartCoroutine(Stage5Camera(timeUntilNextScene / 2));
        }
        StartCoroutine(sceneCont.SpawnNextStage(timeUntilNextScene, stup, stu, stl));        
        col.enabled = true;
    }

    IEnumerator Stage5Camera(float time)
    {
        Text stage5Text = GameObject.FindGameObjectWithTag("Stage5Text").GetComponent<Text>();        
        currentIndex--;
        storyTexts[currentIndex].SetActive(false);
        stage5Text.enabled = true;
        AudioSource audioSource = GetComponent<AudioSource>();
        Camera mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        AudioListener mainCamAL = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();
        AudioListener stage5AL = GameObject.FindGameObjectWithTag("Stage5Camera").GetComponent<AudioListener>();
        Camera stage5Cam = GameObject.FindGameObjectWithTag("Stage5Camera").GetComponent<Camera>();
        mainCam.enabled = false;
        stage5Cam.enabled = true;
        mainCamAL.enabled = false;
        stage5AL.enabled = true;
        screenLight[0].SetActive(true);
        audioSource.Play();
        yield return new WaitForSeconds(time);
        StartCoroutine(StaticEffectScreen(staticEffectTime));
        audioSource.Stop();
        screenLight[0].SetActive(false);
        stage5Cam.enabled = false;
        stage5AL.enabled = false;
        mainCam.enabled = true;
        mainCamAL.enabled = true;
    }

    IEnumerator StaticEffectScreen(float time)
    {
        playerController.playerScreenStatic.SetActive(true);
        yield return new WaitForSeconds(time);
        playerController.playerScreenStatic.SetActive(false);
    }

    private void ResetPlayerPos()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 0, 0);
    }
}
