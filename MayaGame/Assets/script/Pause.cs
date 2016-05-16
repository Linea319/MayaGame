using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Pause : MonoBehaviour {
	public static bool pause =false;
	public GameObject customUI;
	public DepthOfField depth;
	// Use this for initialization
	void Start () {
		depth = gameObject.GetComponent<DepthOfField>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			pause = !pause;
			if(pause){
                if (customUI != null)
                {
                    customUI.SetActive(true);
                }
				depth.enabled = false;
			}
			else{
                if (customUI != null)
                {
                    customUI.SetActive(false);
                }
				depth.enabled = true;
			}
		}
	}
}
