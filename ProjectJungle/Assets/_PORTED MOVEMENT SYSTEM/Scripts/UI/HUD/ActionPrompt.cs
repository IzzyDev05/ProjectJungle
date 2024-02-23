using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionPrompt : MonoBehaviour
{
    public static ActionPrompt Instance;

    private TMP_Text promptText;

    private void Awake()
    {
        #region SINGLETON
        if (Instance != null)
        {
            Debug.LogError("Multiple Action Prompt Instances found.");
        }

        Instance = this;
        #endregion

        promptText = GetComponent<TMP_Text>();
        ClearPrompt();
    }

    public void ClearPrompt()
    {
        promptText.text = "";
    }

    public void PromptPlayer(string message)
    {
        promptText.text = message;
    }
}
