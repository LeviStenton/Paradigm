using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerText : MonoBehaviour {

    public Vector3 offset;
    public Material textDistanceMat;

    Transform player;
    PlayerController playerCont;
    SceneController sceneCont;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerCont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sceneCont = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
    }

    private void FixedUpdate()
    {
        transform.LookAt(player.position + offset);
        this.transform.eulerAngles = new Vector3(Mathf.Clamp(this.transform.eulerAngles.x, -60f, 60f), this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        

        textDistanceMat.SetVector("Vector3_D9CDC386", player.position);
        textDistanceMat.SetVector("Vector3_D07E5377", this.transform.position);        
    }    
}
