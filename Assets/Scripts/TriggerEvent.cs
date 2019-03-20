using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour {

    public GameObject[] screens;
    public GameObject[] screenLight;
    public GameObject[] storyTexts;
    int currentIndex = 0;
    
    PlayerController playerController;

    BoxCollider col;

    public float staticEffectTime;

    //GRAB BOX COLLIDER ON SCREEN TO DEACTIVATE DURING TRANSITION SO INDEX++ DOESN"T INCREASE DURING TRANSITIONS
    public void Start()
    {
        col = this.gameObject.GetComponent<BoxCollider>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            Debug.Log(currentIndex);
        }

        else if(currentIndex >= (storyTexts.Length - 1))
        {
            currentIndex = storyTexts.Length;
            StartCoroutine(StaticEffectEnd(staticEffectTime));
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
    IEnumerator StaticEffectEnd(float time)
    {        
        screens[0].SetActive(false);
        screenLight[0].SetActive(false);
        playerController.playerScreenStatic.SetActive(true);
        yield return new WaitForSeconds(time);
        ResetPlayerPos();
        //SceneManager.LoadScene(nextScene);
        playerController.playerScreenStatic.SetActive(false);        
    }

    private void ResetPlayerPos()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 0, 0);
    }
}
