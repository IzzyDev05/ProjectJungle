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
        UICanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
    }

    public void DeactivateMenu()
    {
        UICanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
    }

    public bool MenuActive { get { return UICanvas.activeSelf == true ? true : false; } }
}
