using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uIManager;
    public MonsterSpawner monsterSpawner;

    private int score = 0;
    public bool IsGameOver { get; private set; }

    private void Start()
    {
        uIManager.SetScore(score);
    }

    public void AddScore(int add)
    {
        if (IsGameOver)
        {
            return;
        }
        score += add;
        uIManager.SetScore(score);
    }

    public void EndGame()
    {
        IsGameOver = true;
        uIManager.SetActiveGameOverUI(true);

        monsterSpawner.OnEndGame();
    }
}
