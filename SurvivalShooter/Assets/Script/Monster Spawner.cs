using UnityEngine;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements.Experimental;

public class MonsterSpawner : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uIManager;
    public Monster monsterPrefab;
    public MonsterData[] monsterDatas;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    private readonly List<Monster> monsters = new List<Monster>();
    private readonly List<Monster> monsterPool = new List<Monster>();

    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("GameComtroller");
        gameManager = findGo.GetComponent<GameManager>();
        uIManager = gameManager.uIManager;
    }

    private void CreateMonster()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        MonsterData data = monsterDatas[Random.Range(0, monsterDatas.Length)];

        Monster monster;
    }
    public void OnEndGame()
    {
        for (int i = monsters.Count - 1; i >= 0; --i)
        {
            monsters[i].Die();
        }
    }

}
