using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewItemViewer : MonoBehaviour
{
    public static NewItemViewer Instance;

    [SerializeField] GameObject itemIconParent;
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

    public void OpenItemViewer(GameObject itemIcon, string name, string lore, float scale, Quaternion rotation)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        GameObject item = Instantiate(itemIcon, itemIconParent.transform);
        item.transform.localScale = item.transform.localScale * scale;
        item.transform.rotation = rotation;

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
