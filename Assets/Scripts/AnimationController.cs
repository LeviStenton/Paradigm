using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public Animator anim;
    public GameObject player;
    public GameObject cam;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        WarpAnim();

        

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            anim.SetBool("isWalking", true);
            
            

        }
        else anim.SetBool("isWalking", false); 
    }

    public void WarpAnim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.tag == "warpActivate")
                {
                    
                    anim.SetBool("warpActivate1", true);
                    Debug.Log("help");
                }
                else anim.SetBool("warpActivate1", false); 

                if (hit.transform.gameObject.tag == "warpActivate1")
                {
                    anim.SetBool("warpActivate2", true);
                }
                else anim.SetBool("warpActivate2", false);

                if (hit.transform.gameObject.tag == "warpActivate2")
                {
                    anim.SetBool("warpActivate3", true);
                }
                else anim.SetBool("warpActivate3", false);

                if (hit.transform.gameObject.tag == "warpActivate3")
                {
                    anim.SetBool("warpActivate4", true);
                }
                else anim.SetBool("warpActivate4", false);
            }            
        }        
    }
}
