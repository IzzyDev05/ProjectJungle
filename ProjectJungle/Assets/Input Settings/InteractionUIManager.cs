using System;
using UnityEngine;
using Cinemachine;

public class InteractionUIManager : MonoBehaviour
{
    private PlayerInteractionAndUIControls IUIControls;
    private InputManager inputManager;

    [SerializeField] private GameObject FreelookCam;
    [SerializeField] private GameObject AimCam;
    private CinemachineInputProvider camLookProvider;
    private CinemachineInputProvider aimLookProvider;

    private bool openInventory = false;
    private bool openUI = false;
    private bool interactInput = false;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();

        camLookProvider = FreelookCam.GetComponent<CinemachineInputProvider>();
        aimLookProvider = AimCam.GetComponent<CinemachineInputProvider>();

    }

    private void OnEnable()
    {
        IUIControls ??= new PlayerInteractionAndUIControls(); // Shorthand for: if (IUIControls == null { --- }


        IUIControls.Player.OpenInventory.performed += i => 
        {
            openInventory = true;
            AudioManager.instance.PlayOneShot(FModEvents.instance.backpack, GameManager.Player.transform.position);
        };

        IUIControls.Player.OpenMenu.performed += i => openUI = true;

        IUIControls.Player.Interact.performed += i => interactInput = true;
        IUIControls.Player.Interact.canceled += i => interactInput = false;

        IUIControls.UI.CloseInventory.performed += i =>
        {
            openInventory = false;
            AudioManager.instance.PlayOneShot(FModEvents.instance.backpack, GameManager.Player.transform.position);
        };

        IUIControls.UI.ExitUI.performed += i => openUI = false;

        IUIControls.Enable();
        IUIControls.UI.Disable();
    }

    private void OnDisable()
    {
        IUIControls.Disable();
    }

    public void HandleUIInputs()
    {
        HandleInventory();
        HandleMenu();
    }

    private void SwitchPlayerToUI(bool reverse = false)
    {
        if (!reverse)
        {
            IUIControls.UI.Enable();
            IUIControls.Player.Disable();

            camLookProvider.enabled = false;
            aimLookProvider.enabled = false;
        }
        else
        {
            IUIControls.Player.Enable();
            IUIControls.UI.Disable();

            camLookProvider.enabled = true;
            aimLookProvider.enabled = true;
        }

        inputManager.DisablePlayerControls(reverse);
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

        UIOpen(openInventory);
    }

    private void HandleMenu()
    {
        if (openUI)
        {
            GameManager.Instance.OpenMenuUI();
        }
        else
        {
            GameManager.Instance.CloseMenuUI();
        }

        //UIOpen(openUI);
    }

    private void UIOpen(bool opened)
    {
        if (opened)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            GameManager.Instance.UnpauseGame();
        }

        SwitchPlayerToUI(!opened);
    }

    public void ButtonCloseUI()
    {
        if (openInventory)
        {
            openInventory = false;
        }

        if (openUI)
        {
            openUI = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string message = "Press 'E' to ";

        if (other.CompareTag("Interact_Pickup"))
        {
            Debug.Log(message + "Pickup");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (interactInput)
        {
            if (other.CompareTag("Interact_Pickup"))
            {
                NewInventoryManager.Instance.AddToInventory(other.gameObject);
            }
        }
    }
}
