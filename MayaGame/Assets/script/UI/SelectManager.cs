using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectManager : MonoBehaviour {
    public Color selectColor;
    public Color defColor;
    public Button[] buttons;
    Button cButton;

	// Use this for initialization
	void Start () {
        cButton = buttons[0];
        PressButton(0);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PressButton(int slot)
    {
        ColorBlock cblock = cButton.colors;
        cblock.normalColor = defColor;
        cButton.colors = cblock;
        cblock.normalColor = selectColor;
        buttons[slot].colors = cblock;
        cButton = buttons[slot];
    } 
}
