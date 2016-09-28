using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadoutChanger : MonoBehaviour {
    public GameObject[] panels;
    GameObject cPanel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPanel(int slot)
    {
        panels[slot].SetActive(true);
        if (cPanel != null) cPanel.SetActive(false);
        cPanel = panels[slot];
    }
}
