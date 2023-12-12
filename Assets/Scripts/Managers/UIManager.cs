using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        Instance = this;
    }

    public void SetGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }
}
