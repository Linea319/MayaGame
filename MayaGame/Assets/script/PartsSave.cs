using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
public class PartsSave : MonoBehaviour{

	public static void SaveParts(parameter parts,string saveSlot){
		string[] paramStr = new string[10];
		paramStr[0] = parts.parameters["damage"].ToString();
		paramStr[1] = parts.parameters["accuracy"].ToString();
		paramStr[2] = parts.parameters["recoil"].ToString();
		paramStr[3] = parts.parameters["range"].ToString();
		paramStr[4] = parts.parameters["mobility"].ToString();
		paramStr[5] = parts.parameters["reload"].ToString();
		paramStr[6] = parts.parameters["weight"].ToString();
		paramStr[7] = ((int)parts.partsType).ToString();
		paramStr[8] = parts.partsName;
		paramStr[9] = parts.specialParam;
		if(parts.partsName == string.Empty){
			parts.partsName = "null";

		}
		SaveText(Application.dataPath+"/Save",@"\"+saveSlot+".txt",paramStr);
//		Debug.Log(Application.dataPath);
	}

	public static parameter LoadParts(string loadname){
		GameObject obj = new GameObject("Cube");
		parameter parts = obj.AddComponent<parameter>();
		string[] paramStr = LoadText(Application.dataPath+"/Save",@"\"+loadname+".txt");
		parts.parameters["damage"] = float.Parse( paramStr[0]);
		parts.parameters["accuracy"] = float.Parse( paramStr[1]);
		parts.parameters["recoil"] = float.Parse( paramStr[2]);
		parts.parameters["range"] = float.Parse( paramStr[3]);
		parts.parameters["mobility"] = float.Parse( paramStr[4]);
		parts.parameters["reload"] = float.Parse( paramStr[5]);
		parts.parameters["weight"] = float.Parse( paramStr[6]);
        int a = int.Parse(paramStr[7]) ;
		parts.partsType =(PartsType)Enum.ToObject(typeof(PartsType),a);
		parts.partsName = paramStr[8];
		parts.specialParam = paramStr[9];
		Destroy(obj);
		return parts;


	}



	public static void SaveText (string fileFolder, string filename, string[] dataStr)
	{
		using (StreamWriter w = new StreamWriter(fileFolder+filename)) {
			foreach (var item in dataStr) {
				w.WriteLine (item);
			}
		}
	}
	public static string[] LoadText (string fileFolder, string filename)
	{
		List<string> strList = new List<string> ();
		string line = "";
		using (StreamReader sr = new StreamReader(fileFolder+filename)) {
			while ((line = sr.ReadLine()) != null) {
				strList.Add (line);
			}
		}
		return strList.ToArray ();
	}
}
