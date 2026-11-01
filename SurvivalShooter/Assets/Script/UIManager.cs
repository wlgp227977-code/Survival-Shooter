using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
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
