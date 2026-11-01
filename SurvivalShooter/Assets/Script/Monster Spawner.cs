using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uIManager;

    public MonsterData[] monsterDatas;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;

    private readonly List<Monster> monsters = new List<Monster>();
    private readonly Dictionary<Monster, MonsterData> monsterPool = new Dictionary<Monster, MonsterData>();

    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("GameController");
        gameManager = findGo.GetComponent<GameManager>();
        uIManager = gameManager.uIManager;

        StartCoroutine(CoSpawn());
    }

    private IEnumerator CoSpawn()
    {
        while (!gameManager.IsGameOver)
        {
            CreateMonster();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void CreateMonster()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        MonsterData data = monsterDatas[Random.Range(0, monsterDatas.Length)];

        Monster monster = null;

        foreach (var pair in monsterPool)
        {
            if (pair.Value == data && !pair.Key.gameObject.activeSelf)
            {
                monster = pair.Key;
                break;
            }
        }

        if (monster == null)
        {
            monster = Instantiate(data.prefab);
            monster.gameObject.SetActive(false);

            monster.OnDead.AddListener(() => monsters.Remove(monster));
            monster.OnDead.AddListener(() => StartCoroutine(CoReturnPool(monster)));

            monsterPool.Add(monster, data);
        }

        monster.Setup(data);
        monster.transform.SetPositionAndRotation(point.position, point.rotation);
        monster.gameObject.SetActive(true);

        monsters.Add(monster);
    }

    private IEnumerator CoReturnPool(Monster monster)
    {
        yield return new WaitForSeconds(5f);

        monster.gameObject.SetActive(false);
    }

    public void OnEndGame()
    {
        for (int i = monsters.Count - 1; i >= 0; --i)
        {
            monsters[i].Die();
        }
    }
}