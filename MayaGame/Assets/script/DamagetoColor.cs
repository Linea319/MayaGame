using UnityEngine;
using System.Collections;

public class DamagetoColor : MonoBehaviour {
    
    [ColorUsage(false,true,0,8,0.125f,3)]public Color fullColor;
    [ColorUsage(false, true, 0, 8, 0.125f, 3)]
    public Color deathColor;
    public HitManagerDef hp;
    public Renderer rend;
    Material mat;
    float h1, s1, v1, h2, s2, v2;
    // Use this for initialization
    void Start () {
        mat = rend.material;
        
        Color.RGBToHSV(fullColor,out h1,out s1,out v1);
        Color.RGBToHSV(deathColor, out h2, out s2, out v2);

    }
	
	// Update is called once per frame
	void Update () {
        float rate = hp.hitPoint / hp.maxHitPoint;
        float newH = Mathf.Lerp(h2, h1, rate);
        Color newColor = Color.HSVToRGB(newH, s1, v1, true);
        mat.SetColor("_EmissionColor",newColor);
	}
}
