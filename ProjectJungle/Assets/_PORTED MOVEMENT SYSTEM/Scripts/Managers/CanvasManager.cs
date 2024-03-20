using UnityEngine;
public class CanvasManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        }
    }
}