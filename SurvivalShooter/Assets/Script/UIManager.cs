using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverUI;

    private void Onable()
    {
        SetActiveGameOverUI(false);
    }

    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }
    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
