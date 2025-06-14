using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Warp Nodes")]
    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    [Header("Audio")]
    [SerializeField] private AudioSource sirenSfx;
    [SerializeField] private AudioSource munch1;
    [SerializeField] private AudioSource munch2;
    [SerializeField] private int currentMunch = 0;

    [Header("Score")]
    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        sirenSfx.Play();
    }

    public void AddToScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }

    public void CollectedPellet(NodeController nodeController)
    {
        if (currentMunch == 0)
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if (currentMunch == 1)
        {
            munch2.Play();
            currentMunch = 0;
        }

        AddToScore(1);
    }
}
