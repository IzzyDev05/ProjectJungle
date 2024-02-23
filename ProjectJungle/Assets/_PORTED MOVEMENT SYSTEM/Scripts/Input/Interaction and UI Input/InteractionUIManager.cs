using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class InteractionUIManager : MonoBehaviour
{
    private PlayerInteractionAndUIControls IUIControls;
    private InputManager inputManager;

    [SerializeField] private GameObject FreelookCam;
    [SerializeField] private GameObject AimCam;
    private CinemachineInputProvider camLookProvider;
    private CinemachineInputProvider aimLookProvider;

    private bool openInventory = false;
    private bool openMenu = false;
    private bool interactInput = false;


    private void Awake()
    {
        foreach (Transform camera in GameObject.Find("Cameras").transform)
        {
            if (camera.name == "FreeLook Camera")
            {
                FreelookCam = camera.gameObject;
            } 
            else if (camera.name == "Aim Camera")
            {
                AimCam = camera.gameObject;
            }
        }
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


        IUIControls.Player.OpenInventory.performed += i => 
        {
            openInventory = true;
            AudioManager.Instance.PlayOneShot(FModEvents.Instance.backpack, GameManager.Player.transform.position);
        };

        IUIControls.Player.OpenMenu.performed += i => openMenu = true;

        IUIControls.Player.Interact.performed += i => interactInput = true;
        IUIControls.Player.Interact.canceled += i => interactInput = false;

        IUIControls.UI.CloseInventory.performed += i =>
        {
            openInventory = false;
            AudioManager.Instance.PlayOneShot(FModEvents.Instance.backpack, GameManager.Player.transform.position);
        };

        IUIControls.UI.CloseMenu.performed += i => openMenu = false;

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

    // INTERACTION
    private void OnTriggerEnter(Collider other)
    {
        string message = $"Press '{GetActionBinds("Interact")}' to ";

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

    // HELPERS
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

    string GetActionBinds(string actionName)
    {
        return IUIControls.FindAction(actionName).bindings[0].ToDisplayString();
    }
}
