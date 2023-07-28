using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystemManager : MonoBehaviour
{
    public static CraftingSystemManager Instance;
    [SerializeField] PlayerInputManager playerInputManager;

    [SerializeField] GameObject UICanvas;
    [SerializeField] GameObject craftingMenuUI;

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

    public bool MenuActive { get { return craftingMenuUI.activeSelf == true ? true : false; } }
}
