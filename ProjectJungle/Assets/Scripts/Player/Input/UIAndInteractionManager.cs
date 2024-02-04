using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class UIAndInteractionManager : MonoBehaviour
{
    private PlayerInteractionAndUIControls UIandInteractionControls;
    private PlayerInputManager playerInput;

    private bool inventoryInput = false;
    private bool menuInput = false;
    private bool closeAllUI = false;

    private bool playOneShot = false;

    private void OnEnable()
    {
        if (UIandInteractionControls == null)
        {
            UIandInteractionControls = new PlayerInteractionAndUIControls();

            UIandInteractionControls.Player.OpenInventory.performed += OpenInventory => { inventoryInput = true; playOneShot = true; };
            UIandInteractionControls.Player.OpenMenu.performed += Menu => menuInput = true;

            UIandInteractionControls.UI.CloseInventory.performed += CloseInventory => { inventoryInput = false; playOneShot = true; };
            UIandInteractionControls.UI.ExitUI.performed += CloseUI => closeAllUI = true;
        }

        UIandInteractionControls.Enable();
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

    /// UI Inputs ///
    private void SwitchInputControls(bool toUIInputs)
    {
        if (toUIInputs == true)
        {
            UIandInteractionControls.Player.Disable();
            UIandInteractionControls.UI.Enable();

            playerInput.DisablePlayerInput();
        }
        else
        {
            playerInput.EnablePlayerInput();

            UIandInteractionControls.UI.Disable();
            UIandInteractionControls.Player.Enable();
        }
    }

    /// <summary>
    /// Closes inventory UI via other means like a button press
    /// </summary>
    public void ManuallyCloseInventory()
    {
        if (inventoryInput == true)
        {
            inventoryInput = false;
            PlayOneShotSound(FModEvents.instance.backpack, transform.position);

            NewInventoryManager.Instance.CloseInventory();
            
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
            //InventoryManager.Instance.OpenInventory();
            NewInventoryManager.Instance.OpenInventory();

            SwitchInputControls(true);

            GameManager.Instance.PauseGame();
        }
    }

    private void OpenSettings()
    {
        SwitchInputControls(true);

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
        if (UIandInteractionControls.UI.enabled == false)
        {
            return;
        }

        if (closeAllUI == true)
        {
            ManuallyCloseInventory();
            CloseSettings();
            closeAllUI = false;
        }

        if (inventoryInput == false)
        {
            //InventoryManager.Instance.CloseInventory();
            NewInventoryManager.Instance.CloseInventory();

            inventoryInput = false;
        }

        playerInput.ResetMovementInput();

        SwitchInputControls(false);
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
        if (other.gameObject.CompareTag("Interact_Pickup") == true)
        {
            Debug.Log($"Press '{GetActionBinds("Interact")}' to pickup {other.gameObject.name}");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Interact_Pickup") == true)
        {
            return;
        }

        if (UIandInteractionControls.Player.Interact.phase == InputActionPhase.Performed)
        {
            /*ItemManager newItem = other.gameObject.GetComponent<ItemManager>();
            InventoryManager.Instance.AddToInventory(newItem.PickupItem(), newItem.AmountPickedUp);*/
            NewInventoryManager.Instance.AddToInventory(other.gameObject);
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
