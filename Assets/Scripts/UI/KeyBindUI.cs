using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindUI : MonoBehaviour
{
    [SerializeField] private Button keyBindButton;
    [SerializeField] private GameInput.KeyBindType keyBindType;
    [SerializeField] private TextMeshProUGUI keyBindText;
    [SerializeField] private GameObject listeningToKeyBindUI;
    [SerializeField] private TextMeshProUGUI listeningToKeyBindText;
    private const string LISTENING_TO_KEY_BIND_TEXT = "Press any button to rebind key...";

    private void Start()
    {
        keyBindText.text = GameInput.Instance.GetKeyBindText(keyBindType);
        keyBindButton.onClick.AddListener(() =>
        {
            keyBindButton.enabled = false;
            listeningToKeyBindText.text =
                "Current key: " + GameInput.Instance.GetKeyBindText(keyBindType) + "\n" + LISTENING_TO_KEY_BIND_TEXT;
            listeningToKeyBindUI.SetActive(true);
            GameInput.Instance.ChangeKeyBind(keyBindType, () =>
            {
                listeningToKeyBindUI.SetActive(false);
                keyBindButton.enabled = true;
            }, keyBindText);
        });
    }
}