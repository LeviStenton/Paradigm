using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWarp : MonoBehaviour {

    public Animation warpAnim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        WarpAnim();		
	}

    public void WarpAnim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                warpAnim.Play();
            }            
        }        
    }
}
