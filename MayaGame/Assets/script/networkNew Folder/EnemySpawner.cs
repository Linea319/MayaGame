using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class EnemySpawner : NetworkBehaviour {
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoint;
    public float[] enemySpawnRate;//合計が100超えないように
    public int defSpawnLimit = 3;
    public int waveSpawnLimit = 10;
    public float spawnRateMin = 3;
    public float spawnRateMax = 9;
    public float waveLengthMin = 30;
    public float waveLengthMax = 90;
    public int sameSpawnMax = 3;
    public LayerMask checkMask;

    //state
    int cSpawnLimit;
    int spawnCount;
    float spawnTimer;
    float WaveTimer;
    bool wave;
    Transform[] players = new Transform[4];
    public int playerNum;
    
    // Use this for initialization

    public override void OnStartServer()
    {
        base.OnStartServer();
        for(int i = 0; i < enemyPrefabs.Length; i++)
        {
            ClientScene.RegisterPrefab(enemyPrefabs[i]);
        }
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        playerNum = playersObj.Length;
        for (int j = 0; j < playersObj.Length; j ++){
            players[j] = playersObj[j].GetComponent<Transform>();
        }
        SwitchWave(false);
    }

    [ServerCallback]
    void Update()
    {
        if(Time.time > spawnTimer)
        {
            int spawnNum = Random.Range(1, sameSpawnMax);
            for(int x = 0; x < spawnNum; x++)
            {
                Spawn();
            }
            spawnTimer = Time.time + Random.Range(spawnRateMin, spawnRateMax);
        }

        if(Time.time > WaveTimer)
        {
            SwitchWave(!wave);
        }
    }

    [ServerCallback]
    public void Spawn()
    {

        if (spawnCount >= cSpawnLimit) return;
        int posId = Random.Range(0, spawnPoint.Length);
        int count = 0;
        while (!CanSpawn(posId) && count < spawnPoint.Length)
        {
            posId++;
            count++;
            if (posId >= spawnPoint.Length) posId = 0;
        }

        float spawnPrefabNum = Random.Range(0, 100f);
        //Debug.Log("SpawnRate" + spawnPrefabNum);
        float spawnCache = 0;
        int prefabId=0;
        for(int i = 0; i < enemyPrefabs.Length; i++)
        {
            if(spawnPrefabNum < enemySpawnRate[i]+ spawnCache)
            {
                prefabId = i;
                break;
            }
            spawnCache += enemySpawnRate[i];
        }

        GameObject enemy = (GameObject)Instantiate(enemyPrefabs[prefabId], spawnPoint[posId].position, spawnPoint[posId].rotation);
        enemy.GetComponent<NetAdapter>().spawnMng = this;
        NetworkServer.Spawn(enemy);
        spawnCount++;
    }

    public void SwitchWave(bool flag)
    {
        wave = flag;
        if (wave)
        {
            cSpawnLimit = waveSpawnLimit;
            WaveTimer = Time.time + Random.Range(waveLengthMin, waveLengthMax);
        }
        else
        {
            cSpawnLimit = defSpawnLimit;
            WaveTimer = Time.time + Random.Range(waveLengthMin/3f, waveLengthMax/3f);
        }
    }

    bool CanSpawn(int spawnId)
    {
        Vector3 spawnPos = spawnPoint[spawnId].position;
        int okNum = 0;
        for (int i = 0; i < playerNum; i++)
        {
            if(Physics.Linecast(spawnPos,players[i].position+Vector3.up,checkMask) || (spawnPos- players[i].position).sqrMagnitude > 50 * 50)
            {
                okNum++;
            }

        }
        if(okNum >= playerNum) return true;

        return false;
    }

    public void deathEnemy()
    {
        spawnCount--;
    }


}
