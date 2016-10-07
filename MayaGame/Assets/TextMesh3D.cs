using UnityEngine;
using System.Collections;

public class TextMesh3D : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Material mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("GUI/Text Shader with Z test");
        mat.color = GetComponent<TextMesh>().color;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
