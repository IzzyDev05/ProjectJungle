using UnityEngine;
using UnityEngine.InputSystem;

using FMODUnity;
using FMOD.Studio;

public class UIAndInteractionManager : MonoBehaviour
{
    private PlayerInteractionAndUIControls UIandInteractionControls;
    private PlayerInputManager playerInput;

    private bool inventoryInput;
    private bool menuInput;
    private bool closeAllUI;
    private bool interactInput;

    private bool playOneShot = false;

    private void OnEnable()
    {
        if (UIandInteractionControls == null)
        {
            UIandInteractionControls = new PlayerInteractionAndUIControls();

            UIandInteractionControls.Player.Interact.performed += Interact => interactInput = true;
            UIandInteractionControls.Player.OpenInventory.performed += OpenInventory => { inventoryInput = true; playOneShot = true; };
            UIandInteractionControls.Player.OpenMenu.performed += Menu => menuInput = true;

            UIandInteractionControls.UI.CloseInventory.performed += CloseInventory => { inventoryInput = false; playOneShot = true; };
            UIandInteractionControls.UI.ExitUI.performed += CloseUI => closeAllUI = true;
        }
    }

    private void OnDisable()
    {
        UIandInteractionControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInputManager>();

        // Disable all UI inputs
        UIandInteractionControls.UI.Disable();

        closeAllUI = false;
    }

    public void HandleUIAndInteractionInputs()
    {
        HandleUIInputs();
    }

    private void SwitchToUIInputs(bool switchToUI)
    {
        if (switchToUI == true)
        {
            UIandInteractionControls.Player.Disable();
            UIandInteractionControls.UI.Enable();
        }
        else
        {
            UIandInteractionControls.UI.Disable();
            UIandInteractionControls.Player.Enable();
        }
    }

    /// UI Inputs ///
    private void SwitchInputs(bool toUIInputs)
    {
        if (toUIInputs)
        {
            SwitchToUIInputs(true);

            playerInput.DisablePlayerInput();
        }
        else
        {
            playerInput.EnablePlayerInput();

            SwitchToUIInputs(false);
        }
    }

    /// <summary>
    /// Closes inventory UI via other means like a button press
    /// </summary>
    public void ManuallyCloseInventory()
    {
        if (inventoryInput)
        {
            inventoryInput = false;
            PlayOneShotSound(FModEvents.instance.backpack, transform.position);
        }
    }

    private void HandleUIInputs()
    {
        CloseUI();

        if (menuInput == true)
        {
            OpenSettings();

            return;
        }

        if (playOneShot == true)
        {
            PlayOneShotSound(FModEvents.instance.backpack, transform.position);
        }

        if (inventoryInput == true)
        {
            InventoryManager.Instance.OpenInventory();

            SwitchInputs(true);

            GameManager.Instance.PauseGame();
        }
    }

    private void OpenSettings()
    {
        SwitchInputs(true);

        GameManager.Instance.PauseGame();

        GameManager.Instance.SettingsUI.SetActive(true);
    }

    private void CloseSettings()
    {
        GameManager.Instance.SettingsUI.SetActive(false);

        menuInput = false;
    }

    private void CloseUI()
    {
        if (!UIandInteractionControls.UI.enabled)
        {
            return;
        }

        if (closeAllUI)
        {
            ManuallyCloseInventory();
            CloseSettings();
            closeAllUI = false;
        }

        if (!inventoryInput)
        {
            InventoryManager.Instance.CloseInventory();

            inventoryInput = false;
        }

        playerInput.ResetMovementInput();

        SwitchInputs(false);
        GameManager.Instance.UnpauseGame();
    }


    // Audio
    private void PlayOneShotSound(EventReference oneshot, Vector3 position)
    {
        AudioManager.instance.PlayOneShot(oneshot, position);
        playOneShot = false;
    }

    // Interactions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interact_Pickup"))
        {
            Debug.Log($"Press '{GetActionBinds("Interact")}' to pickup {other.gameObject.name}");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Interact_Pickup"))
        {
            return;
        }

        if (interactInput)
        {
            ItemManager newItem = other.gameObject.GetComponent<ItemManager>();
            InventoryManager.Instance.AddToInventory(newItem.PickupItem(), newItem.AmountPickedUp);
            interactInput = false;
        }
    }

    // Helpers
    string GetActionBinds(string actionName)
    {
        return UIandInteractionControls.FindAction(actionName).GetBindingDisplayString();
    }

    // UIAndInteractionManager to PlayerInputManager Helpers
    /// <summary>
    /// Checks if the Inventory or Menu is opened.
    /// </summary>
    /// <returns>true if the inventory or menu is open, otherwise return false.</returns>
    public bool IsUIOpened()
    {
        return (inventoryInput || menuInput) == true ? true : false;
    }
}
