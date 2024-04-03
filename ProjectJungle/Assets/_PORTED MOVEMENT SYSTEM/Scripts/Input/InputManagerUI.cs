using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerUI : MonoBehaviour
{
    public static InputManagerUI Instance;
    
    public bool MenuOpenCloseInput { get; private set; }

    private PlayerInput playerInput;
    private InputAction menuOpenCloseAction;

    private void Awake()
    {
        Instance ??= this;

        playerInput = GetComponent<PlayerInput>();
        menuOpenCloseAction = playerInput.actions["MenuOpenClose"];
    }

    private void Update()
    {
        MenuOpenCloseInput = menuOpenCloseAction.WasPressedThisFrame();
    }
}