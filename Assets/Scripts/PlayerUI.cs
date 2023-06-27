using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Text ipText;
    [SerializeField] private TMP_Text survivorCounter;

    public void SetupStartAndIPUI(string IPAddress)
    {
        startGameButton.onClick.AddListener(() => GameManager.Instance.StartGameServerRpc());
        ipText.SetText(IPAddress);
    }

    public void DisableStartAndIPUI()
    {
        startGameButton.gameObject.SetActive(false);
        ipText.gameObject.SetActive(false);
    }

    public void UpdateSurvivorCounter(int newCount)
    {
        survivorCounter.SetText(newCount.ToString());
    }
}
