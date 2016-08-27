using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParamText : MonoBehaviour {
    public Text paramText;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetWepParam(string name)
    {
        GameObject obj = Instantiate(Resources.Load("name", typeof(GameObject))) as GameObject;
        GunParameter aram = obj.GetComponent<Wepon>().pa;
        DamageParameter damParam = obj.GetComponent<Wepon>().dam;
    }

    public void SetMeleeParam(string name)
    {
        GameObject obj = Instantiate(Resources.Load("name", typeof(GameObject))) as GameObject;
        MeleeParameter aram = obj.GetComponent<Melee_Wepon>().pa;
        DamageParameter damParam = obj.GetComponent<Melee_Wepon>().dam;
    }
}
