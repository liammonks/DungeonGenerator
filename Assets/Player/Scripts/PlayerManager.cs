using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [HideInInspector] public PlayerController playerController;

    [SerializeField] private PlayerController playerPrefab = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadPlayer(Vector2 spawnPosition)
    {
        if (playerController != null) { UnloadPlayer(); }
        playerController = Instantiate(playerPrefab, spawnPosition, Quaternion.identity, transform);
        CameraController.Instance.SetTarget(playerController.transform);
    }

    public void UnloadPlayer()
    {
        if (playerController == null) { return; }
        Destroy(playerController.gameObject);
    }
}
