using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystemManager : MonoBehaviour
{
    public static CraftingSystemManager Instance;
    [SerializeField] PlayerInputManager playerInputManager;

    [SerializeField] GameObject UICanvas;
    [SerializeField] GameObject craftingMenuUI;

    [SerializeField] GameObject itemStorage;

    private void Awake()
    {
        Instance = this;


    }

    // Start is called before the first frame update
    void Start()
    {
        DeactivateMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMenu()
    {
        craftingMenuUI.SetActive(true);

        GameManager.Instance.PauseGame();
    }

    public void DeactivateMenu()
    {
        craftingMenuUI.SetActive(false);

        GameManager.Instance.UnpauseGame();
    }

    /// <summary>
    /// Returns the parent that stores all the game objects required for crafting.
    /// </summary>
    public GameObject ItemStorage { get { return itemStorage; } }

    /// <summary>
    /// Returns true if the crafting Menu UI is active.
    /// </summary>
    public bool MenuActive { get { return craftingMenuUI.activeSelf; } }
}
