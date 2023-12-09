using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(OnHostButtonClicked);
        joinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    private void OnHostButtonClicked()
    {
        Debug.Log("HOST");
        NetworkManager.Singleton.StartHost();
        Hide();
    }

    private void OnJoinButtonClicked()
    {
        Debug.Log("JOIN");
        NetworkManager.Singleton.StartClient();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
