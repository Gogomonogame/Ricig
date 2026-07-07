using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    long score = 0;
    [SerializeField] Text scoreText;
    public void ChangeScore(long addScore)
    {
        score += addScore;
        scoreText.text = $"{score}";
    }
}
