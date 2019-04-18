using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour {

    public GameObject worldText;

    PlayerController playerCont;
    SceneController sceneCont;

    void Start()
    {
        playerCont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sceneCont = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && playerCont.stageCount <= 6)
        {

        }

        else if(collision.gameObject.tag == "Player" && playerCont.stageCount >= 6)
        {
            sceneCont.EndGame();
        }
    }
}
