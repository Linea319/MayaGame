using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct compGun{
	public int[] partsNumber;

}

public class Inventory : MonoBehaviour {
	public compGun[] gunslot = new compGun[3];
	public List<parameter> muzuleSlot = new List<parameter>();
	public List<parameter> handGuardSlot = new List<parameter>();
	public List<parameter> upperSlot = new List<parameter>();
	public List<parameter> lowerSlot = new List<parameter>();
	public List<parameter> magwellSlot = new List<parameter>();
	public List<parameter> magazineSlot = new List<parameter>();
	public List<parameter> gripSlot = new List<parameter>();
	public List<parameter> stockSlot = new List<parameter>();
	// Use this for initialization
	void Start () {
		//LoadSlot();
	}
	
	// Update is called once per frame
	 void Update () {
	
	}

	public void SaveGunParts(){
		string[] tmp = new string[8];
		for(int i=0;i<8;i++){
			tmp[i] = gunslot[0].partsNumber[i].ToString();
		}
		PartsSave.SaveText(Application.dataPath+"/Save",@"\"+"gunSlot0"+".txt",tmp);
		for(int i=0;i<8;i++){
			tmp[i] = gunslot[1].partsNumber[i].ToString();
		}
		PartsSave.SaveText(Application.dataPath+"/Save",@"\"+"gunSlot1"+".txt",tmp);
		for(int i=0;i<8;i++){
			tmp[i] = gunslot[2].partsNumber[i].ToString();
		}
		PartsSave.SaveText(Application.dataPath+"/Save",@"\"+"gunSlot2"+".txt",tmp);
	}

	public bool LoadGunParts(){
		string[] tmp = new string[8];
		gunslot[0].partsNumber = new int[8];
		gunslot[1].partsNumber = new int[8];
		gunslot[2].partsNumber = new int[8];
		tmp = PartsSave.LoadText(Application.dataPath+"/Save",@"\"+"gunSlot0"+".txt");
		if(tmp.Length ==0)return false;
		for(int i=0;i<8;i++){
			gunslot[0].partsNumber[i] = int.Parse(tmp[i]);
		}
		tmp = PartsSave.LoadText(Application.dataPath+"/Save",@"\"+"gunSlot1"+".txt");
		if(tmp.Length ==0)return false;
		for(int i=0;i<8;i++){
			gunslot[1].partsNumber[i] = int.Parse(tmp[i]);
		}
		tmp = PartsSave.LoadText(Application.dataPath+"/Save",@"\"+"gunSlot2"+".txt");
		if(tmp.Length ==0)return false;
		for(int i=0;i<8;i++){
			gunslot[2].partsNumber[i] = int.Parse(tmp[i]);
		}
		return true;
	}

	public void SaveSlot(){
		string[] slotInfo= new string[8];
		slotInfo[0] = muzuleSlot.Count.ToString();
		slotInfo[1] = handGuardSlot.Count.ToString();
		slotInfo[2] = upperSlot.Count.ToString();
		slotInfo[3] = lowerSlot.Count.ToString();
		slotInfo[4] = magwellSlot.Count.ToString();
		slotInfo[5] = magazineSlot.Count.ToString();
		slotInfo[6] = gripSlot.Count.ToString();
		slotInfo[7] = stockSlot.Count.ToString();
		PartsSave.SaveText(Application.dataPath+"/Save",@"\"+"SlotInfo"+".txt",slotInfo);
	}

	public bool LoadSlot(){
		string[] slotInfo= PartsSave.LoadText(Application.dataPath+"/Save",@"\"+"SlotInfo"+".txt");
		if(slotInfo.Length==0){
			return false;
		}
		muzuleSlot.Clear();
		handGuardSlot.Clear();
		upperSlot.Clear();
		lowerSlot.Clear();
		magwellSlot.Clear();
		magazineSlot.Clear();
		gripSlot.Clear();
		stockSlot.Clear();
		for(int i=1;i<=int.Parse(slotInfo[0]);i++){
			muzuleSlot.Add (PartsSave.LoadParts("muzule"+i.ToString())); 
		}
        for (int i = 1; i <= int.Parse(slotInfo[1]); i++)
        {
			handGuardSlot.Add (PartsSave.LoadParts("handguard"+i.ToString())); 
		}
        for (int i = 1;i<= int.Parse(slotInfo[2]); i++)
        {
			upperSlot.Add (PartsSave.LoadParts("upper"+i.ToString())); 
		}
        for (int i = 1; i <= int.Parse(slotInfo[3]); i++)
        {
			lowerSlot.Add (PartsSave.LoadParts("lower"+i.ToString())); 
		}
        for (int i = 1; i <= int.Parse(slotInfo[4]); i++)
        {
			magwellSlot.Add (PartsSave.LoadParts("magwell"+i.ToString())); 
		}
        for (int i = 1; i <= int.Parse(slotInfo[5]); i++)
        {
			magazineSlot.Add (PartsSave.LoadParts("magazine"+i.ToString())); 
		}
        for (int i = 1; i <= int.Parse(slotInfo[6]); i++)
        {
			gripSlot.Add (PartsSave.LoadParts("grip"+i.ToString())); 
		}
        for (int i = 1; i <= int.Parse(slotInfo[7]); i++)
        {
			stockSlot.Add (PartsSave.LoadParts("stock"+i.ToString())); 
		}
		return true;
	}

	public void addParts(parameter parts){
		int t;
		switch (parts.partsType){
		case PartsType.muzzle:
			muzuleSlot.Add(parts);
			t = muzuleSlot.Count;
			PartsSave.SaveParts(parts,"muzule"+t.ToString());

			break;
		case PartsType.handguard:
			handGuardSlot.Add(parts);
			t = handGuardSlot.Count;
			PartsSave.SaveParts(parts,"handguard"+t.ToString());
			break;
		case PartsType.upper:
			upperSlot.Add(parts);
			t = upperSlot.Count;
			PartsSave.SaveParts(parts,"upper"+t.ToString());
			break;
		case PartsType.lower:
			lowerSlot.Add(parts);
			t = lowerSlot.Count;
			PartsSave.SaveParts(parts,"lower"+t.ToString());
			break;
		case PartsType.magwell:
			magwellSlot.Add(parts);
			t = magwellSlot.Count;
			PartsSave.SaveParts(parts,"magwell"+t.ToString());
			break;
		case PartsType.magazine:
			magazineSlot.Add(parts);
			t = magazineSlot.Count;
			PartsSave.SaveParts(parts,"magazine"+t.ToString());
			break;
		case PartsType.grip:
			gripSlot.Add(parts);
			t = gripSlot.Count;
			PartsSave.SaveParts(parts,"grip"+t.ToString());
			break;
		case PartsType.stock:
			stockSlot.Add(parts);
			t = stockSlot.Count;
			PartsSave.SaveParts(parts,"stock"+t.ToString());
			break;
		}
		SaveSlot();
	}

	void removeParts(PartsType type,int slot){
		switch (type){
		case PartsType.muzzle:
			muzuleSlot.RemoveAt(slot);
			for(int i=slot;i<=muzuleSlot.Count ;i++){
				PartsSave.SaveParts(muzuleSlot[i],"muzule"+i.ToString()); 
			}
			break;
		case PartsType.handguard:
			handGuardSlot.RemoveAt(slot);
			for(int i=slot;i<=handGuardSlot.Count;i++){
				PartsSave.SaveParts(handGuardSlot[i],"handguard"+i.ToString()); 
			}
			break;
		case PartsType.upper:
			upperSlot.RemoveAt(slot);
			for(int i=slot;i<=upperSlot.Count;i++){
				PartsSave.SaveParts(upperSlot[i],"upper"+i.ToString()); 
			}
			break;
		case PartsType.lower:
			lowerSlot.RemoveAt(slot);
			for(int i=slot;i<=lowerSlot.Count;i++){
				PartsSave.SaveParts(lowerSlot[i],"lower"+i.ToString()); 
			}
			break;
		case PartsType.magwell:
			magwellSlot.RemoveAt(slot);
			for(int i=slot;i<=magwellSlot.Count;i++){
				PartsSave.SaveParts(magwellSlot[i],"magwell"+i.ToString()); 
			}
			break;
		case PartsType.magazine:
			magazineSlot.RemoveAt(slot);
			for(int i=slot;i<=magazineSlot.Count;i++){
				PartsSave.SaveParts(magazineSlot[i],"magazine"+i.ToString()); 
			}
			break;
		case PartsType.grip:
			gripSlot.RemoveAt(slot);
			for(int i=slot;i<=gripSlot.Count;i++){
				PartsSave.SaveParts(gripSlot[i],"grip"+i.ToString()); 
			}
			break;
		case PartsType.stock:
			stockSlot.RemoveAt(slot);
			for(int i=slot;i<=stockSlot.Count;i++){
				PartsSave.SaveParts(stockSlot[i],"stock"+i.ToString()); 
			}
			break;
		}
	}




}
