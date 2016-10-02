using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParamText : MonoBehaviour {
    public Text paramText;
    public Transform ParamParent;
    public GameObject GunPrefab;
    public GameObject MeleePrefab;
    GameObject cTextObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetWepParam(string name)
    {
        Destroy(cTextObj);
        GameObject obj = Instantiate(Resources.Load(name, typeof(GameObject))) as GameObject;
        GunParameter param = obj.GetComponent<Wepon>().pa;
        DamageParameter damParam = obj.GetComponent<Wepon>().dam;
        cTextObj = Instantiate(GunPrefab) as GameObject;
        cTextObj.transform.SetParent(ParamParent,false);
        cTextObj.GetComponent<ParamTextSetter>().SetGunParam(obj.name, param, damParam);
        Destroy(obj);
    }

    public void SetMeleeParam(string name)
    {
        Destroy(cTextObj);
        GameObject obj = Instantiate(Resources.Load(name, typeof(GameObject))) as GameObject;
        MeleeParameter param = obj.GetComponent<Melee_Wepon>().pa;
        DamageParameter damParam = obj.GetComponent<Melee_Wepon>().dam;
        cTextObj = Instantiate(MeleePrefab) as GameObject;
        cTextObj.transform.SetParent(ParamParent,false);
        cTextObj.GetComponent<ParamTextSetter>().SetMeleeParam(obj.name, param, damParam);
        Destroy(obj);
    }

    public void SetAtachParam(string name)
    {
        if (name.Length <= 0)
        {
            cTextObj.GetComponent<ParamTextSetter>().ResetHosei();
            return;
        }
        GameObject obj = Instantiate(Resources.Load(name, typeof(GameObject))) as GameObject;
        GunParameter param = obj.GetComponent<Atachment>().hosei;
        cTextObj.GetComponent<ParamTextSetter>().SetHosei(param);
        Destroy(obj);
    }
}
