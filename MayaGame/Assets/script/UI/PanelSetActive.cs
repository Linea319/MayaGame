using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelSetActive : MonoBehaviour {
    public GameObject panel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetActive(bool active)
    {
        panel.SetActive(active);
    }
}
