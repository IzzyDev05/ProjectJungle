using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewItemViewer : MonoBehaviour
{
    public static NewItemViewer Instance;

    [SerializeField] Image icon;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;


    private void Awake()
    {
        #region SINGLETON
        if (Instance != null)
        {
            Debug.LogError("Multiple Item Viewer Instances found.");
        }

        Instance = this;
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void OpenItemViewer(Sprite image, string name, string lore)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        icon.sprite = image;
        nameText.text = name;
        descriptionText.text = lore;
    }

    public void HideItemViewer()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
    }
}
