using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Phase
{
    public float PhaseTime;

    public Phase(float phaseTime)
    {
        this.PhaseTime = phaseTime;
    }
}

public class MonsterSpawner : MonoBehaviour
{
    public List<Transform> SpawnPoints;
    public float outOfScreenXPos = -20f;
    public List<GameObject> MonsterList;
    public float scrollSpeed;
    public float monsterSpace;
    public float initialXPos;
    public Phase currentPhase;
    [SerializeField] private float targetXPos;

    public static float currentSpace;
    // 1 Phase = 5분 = 360f 스크롤 시간과 관계 없음;
    // 
    
    void Start()
    {
        Init();
    }

    void Init()
    {
        targetXPos = this.transform.position.x + initialXPos;
        currentSpace = 0;
        if (currentPhase == null)
        {
            currentPhase = new Phase(360f);
        }
    }
    
    void MonsterSpawn()
    {
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            var monster = Instantiate(MonsterList[i], SpawnPoints[i]);
            monster.transform.position += new Vector3(currentSpace, 0f, 0f);
        }

        currentSpace += monsterSpace;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x - scrollSpeed * Time.deltaTime, currentPosition.y, 0f);
        transform.position = newPosition;    

        if (transform.position.x <= targetXPos)
        {
            targetXPos -= monsterSpace;
            MonsterSpawn();
        }
    }
    
}
