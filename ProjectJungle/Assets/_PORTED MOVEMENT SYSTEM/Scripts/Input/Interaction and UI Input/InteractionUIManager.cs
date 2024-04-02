using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.LowLevel;

public class InteractionUIManager : MonoBehaviour
{
    private PlayerInteractionAndUIControls IUIControls;
    private InputManager inputManager;

    #region CAMERA
    [SerializeField] private GameObject FreelookCam;
    [SerializeField] private GameObject AimCam;
    private CinemachineInputProvider camLookProvider;
    private CinemachineInputProvider aimLookProvider;
    #endregion

    #region INPUT_BOOLEANS
    private bool openInventory = false;
    private bool openMenu = false;
    private bool interactInput = false;
    #endregion

    private ActionPrompt actionPrompter;
    private GameObject uiBlur;

    int deviceScheme = -1;

    private void Awake()
    {
        foreach (Transform camera in GameObject.Find("Cameras").transform)
        {
            if (camera.name == "FreeLook Camera")
            {
                FreelookCam = camera.gameObject;
            } 
            else if (camera.name.Contains("Aim"))
            {
                AimCam = camera.gameObject;
            }
        }

        actionPrompter = GameObject.Find("ActionPrompt").GetComponent<ActionPrompt>();
        //uiBlur = GameObject.Find("UI Blur");
    }

    private void Start()
    {
        inputManager = GetComponent<InputManager>();

        camLookProvider = FreelookCam.GetComponent<CinemachineInputProvider>();
        aimLookProvider = AimCam.GetComponent<CinemachineInputProvider>();
    }

    private void OnEnable()
    {
        IUIControls ??= new PlayerInteractionAndUIControls(); // Shorthand for: if (IUIControls == null { --- }

        #region PLAYER_ACTION_INPUTS
        IUIControls.Player.OpenInventory.performed += i => 
        {
            openInventory = true;
            AudioManager.Instance.PlayOneShot(FModEvents.Instance.backpack, GameManager.Player.transform.position);
        };

        IUIControls.Player.OpenMenu.performed += i => openMenu = true;

        IUIControls.Player.Interact.performed += i => interactInput = true;
        IUIControls.Player.Interact.canceled += i => interactInput = false;
        #endregion

        #region UI_ACTION_INPUTS
        IUIControls.UI.CloseInventory.performed += i =>
        {
            openInventory = false;
            AudioManager.Instance.PlayOneShot(FModEvents.Instance.backpack, GameManager.Player.transform.position);
        };

        IUIControls.UI.CloseMenu.performed += i => openMenu = false;
        #endregion

        IUIControls.Enable();
        IUIControls.UI.Disable();
    }

    private void OnDisable()
    {
        IUIControls.Disable();
    }

    #region HANDLE_UI_INPUT_FUNCTIONS
    public void HandleUIInputs()
    {
        HandleInventory();
        HandleMenu();

        UIOpen(openInventory ^ openMenu); // openInventory XOR openMenu
    }

    private void HandleInventory()
    {
        if (openInventory)
        {
            NewInventoryManager.Instance.OpenInventory();
        }
        else
        {
            NewInventoryManager.Instance.CloseInventory();
        }
    }

    private void HandleMenu()
    {
        if (openMenu)
        {
            GameManager.Instance.OpenMenuUI();
        }
        else
        {
            GameManager.Instance.CloseMenuUI();
        }
    }
    #endregion

    #region INTERACTION
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interact_Pickup"))
        {
            PromptMessage("Action");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (interactInput)
        {
            if (other.CompareTag("Interact_Pickup"))
            {
                NewInventoryManager.Instance.AddToInventory(other.gameObject);
                actionPrompter.ClearPrompt();
            }
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        actionPrompter.ClearPrompt();
    }
    #endregion

    #region HELPERS
    /// <summary>
    /// Disables the player controls for the input system and switched the the UI controls.
    /// </summary>
    /// <param name="reverse"></param>
    private void SwitchPlayerToUI(bool reverse = false)
    {
        if (!reverse)
        {
            IUIControls.UI.Enable();
            IUIControls.Player.Disable();

            camLookProvider.enabled = false;
            //aimLookProvider.enabled = false;
        }
        else
        {
            IUIControls.Player.Enable();
            IUIControls.UI.Disable();

            camLookProvider.enabled = true;
            //aimLookProvider.enabled = true;
        }

        inputManager.DisablePlayerControls(reverse);
    }

    /// <summary>
    /// Helper function that pauses and switches controls.
    /// </summary>
    /// <param name="opened">Boolean for determining whether the UI is open or close.</param>
    private void UIOpen(bool opened)
    {
        if (opened)
        {
            //uiBlur.gameObject.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        else
        {
            //uiBlur.gameObject.SetActive(false);
            GameManager.Instance.UnpauseGame();
        }

        SwitchPlayerToUI(!opened);
    }

    /// <summary>
    /// Prompts the player depending on the action given.
    /// </summary>
    /// <param name="action">String which will be the action the player can perform. The action will also be prompted to the player as part of the message.</param>
    private void PromptMessage(string action)
    {
        actionPrompter.PromptPlayer($"Press '{GetActionBinds("Interact")}' to {action}");
    }

    /// <summary>
    /// Gets the binding for an action
    /// </summary>
    /// <param name="actionName">The name of the action</param>
    /// <returns>The binding of the action.</returns>
    private string GetActionBinds(string actionName)
    {
        deviceScheme = 0;

        if (deviceScheme == -1)
        {
            //Debug.LogError($"Current Device does not exist. deviceScheme returns {deviceScheme}.");
            throw new Exception($"Current Device does not exist. deviceScheme returns {deviceScheme}.");
        }

        return IUIControls.FindAction(actionName).GetBindingDisplayString(); //IUIControls.FindAction(actionName).bindings[deviceScheme].ToDisplayString();
    }
    #endregion

    #region PUBLIC_EXTERNAL_FUNCTIONS
    public void ButtonCloseUI()
    {
        if (openInventory)
        {
            openInventory = false;
        }

        if (openMenu)
        {
            openMenu = false;
        }
    }
    #endregion
}
