using UnityEngine;
public class CanvasManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        canvas.worldCamera = Camera.main;
    }
}