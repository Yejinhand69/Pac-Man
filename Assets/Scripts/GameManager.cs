using System.Collections;
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

    [Header("Pacman Reference")]
    public GameObject pacman;

    [Header("Ghost Nodes")]
    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    [Header("Coroutine")]
    private Coroutine readyToLeaveHomeCoroutine;

    [Header("Enemies")]
    [SerializeField] private EnemyController[] enemies;

    [Header("UI References")]
    [SerializeField] private GameObject ghostAreOutText;

    private void Awake()
    {
        sirenSfx.Play();
        //ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true;

        readyToLeaveHomeCoroutine = StartCoroutine(ReadyToLeaveHome());
        ghostAreOutText.SetActive(false);
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

    private IEnumerator ReadyToLeaveHome()
    {
        //Wait for 5 seconds before allowing ghosts to leave their home
        yield return new WaitForSeconds(5f);

        //Let the ghosts know they are ready to leave home
        foreach (var ghost in enemies)
        {
            ghost.readyToLeaveHome = true;
            ghostAreOutText.SetActive(true);
        }

        yield return new WaitForSeconds(3f);
        ghostAreOutText.SetActive(false);
    }
}
