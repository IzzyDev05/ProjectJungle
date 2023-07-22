using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemPanelManager : MonoBehaviour
{
    public static ItemPanelManager Instance;

    [SerializeField] GameObject itemPanel;

    [SerializeField] Image itemImage;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text worthText;

    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text defeseText;
    [SerializeField] TMP_Text attackSpeedText;
    [SerializeField] TMP_Text rangeText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] ItemObject currentItem;

    void Awake()
    {
        Instance = this;

        SetToDefault();
    }

    /*/ Start is called before the first frame update
    void Start()
    {
        
    }*/

    // Update is called once per frame
    void Update()
    {
        if (currentItem == null)
        {
            nameText.text = "";
            worthText.text = "";
            damageText.text = "";
            defeseText.text = "";
            attackSpeedText.text = "";
            rangeText.text = "";
            descriptionText.text = "";
        }
    }

    public void DisplaySelectedItem(FoodObject selectedFood)
    {
        if (currentItem != selectedFood)
        {

        }
    }

    public void DisplaySelectedItem(EquipmentObject selectedEqipment)
    {
        if (itemPanel.activeSelf == false)
        {
            itemPanel.SetActive(true);
        }

        if (currentItem == selectedEqipment)
        {
            return;   
        }

        itemImage.sprite = selectedEqipment.GetIcon;

        currentItem = selectedEqipment;

        nameText.text = selectedEqipment.GetItemName;

        worthText.text = selectedEqipment.GetWorth.ToString() + " g";

        if (selectedEqipment.GetDamage != 0)
        {
            damageText.text = selectedEqipment.GetDamage.ToString();
        }
        
        if (selectedEqipment.GetDefense != 0)
        {
            defeseText.text = selectedEqipment.GetDefense.ToString();
        }

        if (selectedEqipment.GetAttackSpeed != 0f)
        {
            attackSpeedText.text = selectedEqipment.GetAttackSpeed.ToString();
        }

        if (selectedEqipment.GetRange != 0f)
        {
            rangeText.text = selectedEqipment.GetRange.ToString();
        }

        if (selectedEqipment.GetDescription != "")
        {
            descriptionText.text = selectedEqipment.GetDescription;
        }
    }

    public void DisplaySelectedItem(MobDropObject selectedMobDrop)
    {
        if (currentItem != selectedMobDrop)
        {

        }
    }

    public void ClearDisplay()
    {
        if (currentItem != null)
        {
            currentItem = null;
        }

        SetToDefault();

    }

    void SetToDefault()
    {
        nameText.text = "";
        worthText.text = "";
        damageText.text = "";
        defeseText.text = "";
        attackSpeedText.text = "";
        rangeText.text = "";
        descriptionText.text = "";

        if (itemPanel.activeSelf == true)
        {
            itemPanel.SetActive(false);
        }
    }
}
