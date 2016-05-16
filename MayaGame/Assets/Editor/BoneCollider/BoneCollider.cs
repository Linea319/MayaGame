using UnityEngine;
using UnityEditor; // エディタ拡張関連はUnityEditor名前空間に定義されているのでusingしておく。
using System.Collections;

// エディタに独自のウィンドウを作成する
public class BoneCollider : EditorWindow
{
    // メニューのWindowにEditorExという項目を追加。
    [MenuItem("Window/EditorEx")]
    static void Open()
    {
        // メニューのWindow/EditorExを選択するとOpen()が呼ばれる。
        // 表示させたいウィンドウは基本的にGetWindow()で表示＆取得する。
        EditorWindow.GetWindow<BoneCollider>("BoneCol"); // タイトル名を"EditorEx"に指定（後からでも変えられるけど）
    }
    int fieldLength=1;
    Object[] objectField = new Object[1];
    float[] radiusRate = { 0.2f };

    void OnGUI()
    {
        EditorGUILayout.LabelField("Bone to Collider");
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            fieldLength = EditorGUILayout.DelayedIntField("Bone Num",fieldLength);
            if (objectField.Length != fieldLength)
            {
                Object[] cObj = new Object[fieldLength];
                float[] cRate = new float[fieldLength];
                
                if (objectField.Length > fieldLength)
                {
                    for (int i = 0; i < fieldLength; i++)
                    {
                        cObj[i] = objectField[i];
                        cRate[i] = radiusRate[i];
                    }
                }
                else
                {
                    for (int i = 0; i < fieldLength; i++)
                    {
                        cRate[i] = 0.2f;
                    }
                    objectField.CopyTo(cObj, 0);
                    radiusRate.CopyTo(cRate, 0);
                    
                }
                    
                
                
                objectField = cObj;
                radiusRate = cRate;
            }
            for (int i = 0; i < fieldLength; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    objectField[i] = EditorGUILayout.ObjectField("Bone" + i, objectField[i], typeof(Transform), true);
                    radiusRate[i] = EditorGUILayout.FloatField(radiusRate[i]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate"))
        {
            Debug.Log("Generate");
            Generate();
        }
    }

    void Generate()
    {
        for(int i=0;i< objectField.Length; i++)
        {
            if (objectField[i] == null) continue;//設定されてなければスキップ
            Transform tr = objectField[i] as Transform;
            Vector3 othePos = tr.parent.position;
            Vector3 colCenter = Vector3.zero;
            float distance = 1f;
            if(othePos != null)
            {
                colCenter = (othePos - tr.position);
                distance = (tr.position - othePos).magnitude;
            }
            CapsuleCollider col = tr.parent.gameObject.AddComponent<CapsuleCollider>();
            col.center = new Vector3(0,distance*0.5f,0);
            col.height = distance;
            col.radius = distance * radiusRate[i];
            

        }
    }


}