using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public string sceneToLoad;
    PlayerController playerCont;
    SceneController sceneCont;

    // Start is called before the first frame update
    void Start()
    {        
        playerCont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sceneCont = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && playerCont.stageCount >= 6)
        {
            StartCoroutine(sceneCont.FadeToBlackEnd(3, sceneToLoad));
            Debug.Log("Hit Wall");
        }
    }
}
