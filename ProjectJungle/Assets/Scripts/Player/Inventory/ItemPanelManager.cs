using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemPanelManager : MonoBehaviour
{
    public static ItemPanelManager itemPanelManager;

    [SerializeField] GameObject statsPanel;

    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text defeseText;
    [SerializeField] TMP_Text attackSpeedText;
    [SerializeField] TMP_Text rangeText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] ItemObject currentItem;

    void Awake()
    {
        itemPanelManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (statsPanel.activeSelf == true)
        {
            statsPanel.SetActive(false);
        }

        damageText.text = "";
        defeseText.text = "";
        attackSpeedText.text = "";
        rangeText.text = "";
        descriptionText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (currentItem == null)
        {
            damageText.text = "";
            defeseText.text = "";
            attackSpeedText.text = "";
            rangeText.text = "";
            descriptionText.text = "";
        }
    }

    public void DisplaySelectedItem(ItemObject selectedItem)
    {
        if (currentItem != selectedItem)
        {
            

        }
    }

    public void ClearDisplay()
    {
        if (currentItem != null)
        {
            currentItem = null;
        }
    }
}
