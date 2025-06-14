using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Warp Nodes")]
    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    [Header("Audio")]
    [SerializeField] private AudioSource sirenSfx;

    private void Awake()
    {
        sirenSfx.Play();
    }
}
