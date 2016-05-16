using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;  

public class GunCustom : MonoBehaviour {
	public Dictionary<string,GameObject> gunParts = new Dictionary<string,GameObject>();
	[SerializeField]
	RectTransform nodePrefab = null;
	[SerializeField]
	RectTransform nodeParent;
	public Text[] allParams;
	public gun myGun;
	public Inventory inventry;
	public int[] slotNumbers = new int[8];
	public Color positiveColor;
	int useSlot;
	parameter prevParam;

	int nowGunSlot = 0;
	// Use this for initialization
	void Start () {
		LoadSlot(0);
		//SetPrevParam(myGun);
		//prevParam = myGun;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0;i<myGun.useParams.Length;i++){
			float tmp = myGun.parameters[myGun.useParams[i]] - prevParam.parameters[myGun.useParams[i]];
			if(tmp<=-0.5){
			allParams[i].text = myGun.useParams[i]+":"+myGun.parameters[myGun.useParams[i]].ToString("f0")
				+"[<color=red>"+tmp.ToString("f0")+"</color>]";
			}
			else{
				allParams[i].text = myGun.useParams[i]+":"+myGun.parameters[myGun.useParams[i]].ToString("f0")
					+"[<color=lime>+"+tmp.ToString("f0")+"</color>]";
			}
		}

		if(Input.GetKeyDown(KeyCode.F1)){
			GameObject[] obj = Resources.LoadAll<GameObject>("parts");
			PlusParts(obj[UnityEngine.Random.Range(0,obj.Length)]);
		}

	}

	public void PlusParts(GameObject partsObj){
		parameter p =partsObj.GetComponent<parameter>();
		p.DivideParm();
		inventry.addParts(p);
	}

	public void LoadSlot(int slotNum){

		if(inventry.LoadGunParts()){
		slotNumbers = inventry.gunslot[slotNum].partsNumber;
		}
		else{
			slotNumbers = new int[8];
		}
		StartCoroutine("CreateGun");
	}

	public void SaveSlot(int slotNum){
		inventry.gunslot[slotNum].partsNumber = slotNumbers; 
		inventry.SaveGunParts();
	}



	public IEnumerator CreateGun(){
		foreach ( Transform n in myGun.transform )
		{
			GameObject.Destroy(n.gameObject);
		}
		yield return new WaitForEndOfFrame();
		bool first = inventry.LoadSlot();
		Debug.Log(first);                                 
		if(!first){
			gunParts["muzzle"] = Instantiate(Resources.Load("parts/"+"muzzle"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["muzzle"], null, (x,y)=>x.DivideParm());
			gunParts["handguard"] =Instantiate(Resources.Load("parts/"+"handGuard"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["handguard"], null, (x,y)=>x.DivideParm());
			gunParts["upper"] =Instantiate(Resources.Load("parts/"+"upReciever"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["upper"], null, (x,y)=>x.DivideParm());
			gunParts["lower"] =Instantiate(Resources.Load("parts/"+"lower"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["lower"], null, (x,y)=>x.DivideParm());
			gunParts["magwell"] =Instantiate(Resources.Load("parts/"+"magwell"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["magwell"], null, (x,y)=>x.DivideParm());
			gunParts["magazine"] =Instantiate(Resources.Load("parts/"+"mag"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["magazine"], null, (x,y)=>x.DivideParm());
			gunParts["grip"] =Instantiate(Resources.Load("parts/"+"grip"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["grip"], null, (x,y)=>x.DivideParm());
			gunParts["stock"] =Instantiate(Resources.Load("parts/"+"stock"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["stock"], null, (x,y)=>x.DivideParm());
		}
		else{
			gunParts["muzzle"] = Instantiate(Resources.Load("parts/"+inventry.muzuleSlot[slotNumbers[0]].partsName))as GameObject;
			gunParts["handguard"] =Instantiate(Resources.Load("parts/"+inventry.handGuardSlot[slotNumbers[1]].partsName))as GameObject;
			gunParts["upper"] =Instantiate(Resources.Load("parts/"+inventry.upperSlot[slotNumbers[2]].partsName))as GameObject;
			gunParts["lower"] =Instantiate(Resources.Load("parts/"+inventry.lowerSlot[slotNumbers[3]].partsName))as GameObject;
			gunParts["magwell"] =Instantiate(Resources.Load("parts/"+inventry.magwellSlot[slotNumbers[4]].partsName))as GameObject;
			gunParts["magazine"] =Instantiate(Resources.Load("parts/"+inventry.magazineSlot[slotNumbers[5]].partsName))as GameObject;
			gunParts["grip"] =Instantiate(Resources.Load("parts/"+inventry.gripSlot[slotNumbers[6]].partsName))as GameObject;
			gunParts["stock"] =Instantiate(Resources.Load("parts/"+inventry.stockSlot[slotNumbers[7]].partsName))as GameObject;
		}
		Transform guntr = myGun.GetComponent<Transform>();
		gunParts["muzzle"].transform.SetParent(guntr,false);
		gunParts["handguard"].transform.SetParent(guntr,false);
		gunParts["upper"].transform.SetParent(guntr,false);
		gunParts["lower"].transform.SetParent(guntr,false);
		gunParts["magwell"].transform.SetParent(guntr,false);
		gunParts["magazine"].transform.SetParent(guntr,false);
		gunParts["grip"].transform.SetParent(guntr,false);
		gunParts["stock"].transform.SetParent(guntr,false);
		gunParts["lower"].transform.localPosition = Vector3.zero;
		gunParts["lower"].transform.localEulerAngles = Vector3.zero;
		gunParts["upper"].transform.position = gunParts["lower"].transform.FindChild("upperSlot").position;
		gunParts["stock"].transform.position = gunParts["lower"].transform.FindChild("stockSlot").position;
		gunParts["grip"].transform.position = gunParts["lower"].transform.FindChild("gripSlot").position;
		gunParts["magwell"].transform.position = gunParts["lower"].transform.FindChild("magwellSlot").position;
		gunParts["handguard"].transform.position = gunParts["upper"].transform.FindChild("handguardSlot").position;
		gunParts["muzzle"].transform.position = gunParts["handguard"].transform.FindChild("muzzleSlot").position;
		Debug.Log("set");
        if (!first)
        {
            inventry.addParts(gunParts["muzzle"].GetComponent<parameter>());
            inventry.addParts(gunParts["handguard"].GetComponent<parameter>());
            inventry.addParts(gunParts["upper"].GetComponent<parameter>());
            inventry.addParts(gunParts["lower"].GetComponent<parameter>());
            inventry.addParts(gunParts["magwell"].GetComponent<parameter>());
            inventry.addParts(gunParts["magazine"].GetComponent<parameter>());
            inventry.addParts(gunParts["grip"].GetComponent<parameter>());
            inventry.addParts(gunParts["stock"].GetComponent<parameter>());
            inventry.SaveSlot();
			SaveSlot(0);
			SaveSlot(1);
			SaveSlot(2);
        }
        else
        {
            ExecuteEvents.Execute<ParamReciever>(gunParts["muzzle"], null, (x, y) => x.LoadParam(inventry.muzuleSlot[slotNumbers[0]]));
            ExecuteEvents.Execute<ParamReciever>(gunParts["handguard"], null, (x, y) => x.LoadParam(inventry.handGuardSlot[slotNumbers[1]]));
            ExecuteEvents.Execute<ParamReciever>(gunParts["upper"], null, (x, y) => x.LoadParam(inventry.upperSlot[slotNumbers[2]]));
            ExecuteEvents.Execute<ParamReciever>(gunParts["lower"], null, (x, y) => x.LoadParam(inventry.lowerSlot[slotNumbers[3]]));
            ExecuteEvents.Execute<ParamReciever>(gunParts["magwell"], null, (x, y) => x.LoadParam(inventry.magwellSlot[slotNumbers[4]]));
            ExecuteEvents.Execute<ParamReciever>(gunParts["magazine"], null, (x, y) => x.LoadParam(inventry.magazineSlot[slotNumbers[5]]));
            ExecuteEvents.Execute<ParamReciever>(gunParts["grip"], null, (x, y) => x.LoadParam(inventry.gripSlot[slotNumbers[6]]));
            ExecuteEvents.Execute<ParamReciever>(gunParts["stock"], null, (x, y) => x.LoadParam(inventry.stockSlot[slotNumbers[7]]));
        }

        myGun.CombineParam();
		LoadPrevParam();

	}

	public void ChangeParts(int partsNum){
		slotNumbers[useSlot] = partsNum;
		SetPrevParam(myGun);
		StartCoroutine("CreateGun");
		SaveSlot(nowGunSlot);
		PartsType tmpType = (PartsType)Enum.ToObject(typeof(PartsType),useSlot);
		SetButton(tmpType.ToString());

	}

	void SetPrevParam(gun tmpGun){
		/*for(int i=0;i<tmpGun.useParams.Length;i++){
			prevParam.parameters[tmpGun.useParams[i]] = myGun.parameters[tmpGun.useParams[i]];
		}*/
		PartsSave.SaveParts(tmpGun,"prevParam");
	}

	void LoadPrevParam(){
		prevParam = PartsSave.LoadParts("prevParam");
	}

	public void SetButton(string typestr){
		PartsType type = (PartsType)Enum.Parse(typeof(PartsType),typestr);
		foreach ( Transform n in nodeParent )
		{
			GameObject.Destroy(n.gameObject);
		}
		useSlot = (int)type;
		switch (type){
		case PartsType.muzzle:
			for(int i=0; i<inventry.muzuleSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.muzuleSlot[i].partsName;
				if(i == slotNumbers[0]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		case PartsType.handguard:
			for(int i=0; i<inventry.handGuardSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.handGuardSlot[i].partsName;
				if(i == slotNumbers[1]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		case PartsType.upper:
			for(int i=0; i<inventry.upperSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.upperSlot[i].partsName;
				if(i == slotNumbers[2]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		case PartsType.lower:
			for(int i=0; i<inventry.lowerSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.lowerSlot[i].partsName;
				if(i == slotNumbers[3]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		case PartsType.magwell:
			for(int i=0; i<inventry.magwellSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.magwellSlot[i].partsName;
				if(i == slotNumbers[4]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		case PartsType.magazine:
			for(int i=0; i<inventry.magazineSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.magazineSlot[i].partsName;
				if(i == slotNumbers[5]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		case PartsType.grip:
			for(int i=0; i<inventry.gripSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.gripSlot[i].partsName;
				if(i == slotNumbers[6]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		case PartsType.stock:
			for(int i=0; i<inventry.stockSlot.Count; i++)
			{
				var item = GameObject.Instantiate(nodePrefab) as RectTransform;
				item.SetParent(nodeParent, false);
				var text = item.GetComponentInChildren<Text>();
				text.text = inventry.stockSlot[i].partsName;
				if(i == slotNumbers[7]){
					text.color = positiveColor;
				}
				item.SendMessage("SetNum",i);
			}
			break;
		}

	}


}
