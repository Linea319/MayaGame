using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AtachmentSpawn : MonoBehaviour {
    public GameObject[] prefabs;
    public Transform panel;
    public Transform lobbyPanel;
    public int slot;
    // Use this for initialization
    public void SpawnAtach()
    {
        int num = panel.childCount;
        for (int i = 0; i < num; i++)
        {
            Destroy(panel.GetChild(i).gameObject);
        }
        SelectManager sMng = panel.GetComponent<SelectManager>();
            for (int i = 0; i < prefabs.Length; i++)
        {
            GameObject obj = Instantiate(prefabs[i]);
            obj.transform.SetParent(panel,false);
            obj.GetComponent<setAtachment>().initialize(lobbyPanel, slot,i);
            sMng.buttons[i] = obj.GetComponent<Button>();
        }
        sMng.Initialize();
    }
}
