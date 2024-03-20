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

    /// <summary>
    /// Clear the text for the action prompt
    /// </summary>
    public void ClearPrompt()
    {
        promptText.text = "";
    }

    /// <summary>
    /// Shows the prompt to the player.
    /// </summary>
    /// <param name="message">Message that will be displayed to the player.</param>
    public void PromptPlayer(string message)
    {
        promptText.text = message;
    }
}
