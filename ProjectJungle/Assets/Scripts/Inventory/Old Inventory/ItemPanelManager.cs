using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemPanelManager : MonoBehaviour
{
    public static ItemPanelManager Instance;

    [SerializeField] GameObject itemPanel;
    [SerializeField] GameObject equipmentPanel;

    [SerializeField] Image itemImage;

    [SerializeField] string itemTextPath;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text worthText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text defeseText;
    [SerializeField] TMP_Text attackSpeedText;
    [SerializeField] TMP_Text rangeText;

    [SerializeField] ItemObject currentItem;

    void Awake()
    {
        #region SIMPLETON
        if (Instance != null)
        {
            Debug.LogError("Multiple Item Panel Manager Instances found.");
        }

        Instance = this;
        #endregion

        SetToDefault();
    }

    /// <summary>
    /// Display generic Item
    /// </summary>
    /// <param name="selectedItem"></param>
    public void DisplaySelectedItem(ItemObject selectedItem)
    {
        if (itemPanel.activeSelf == false)
        {
            itemPanel.SetActive(true);
        }

        if (currentItem == selectedItem)
        {
            return;
        }

        currentItem = selectedItem;

        GetPanelFields();

        SetItemPanel(selectedItem);
    }

    /// <summary>
    /// Overloaded DisplaySelectedItem for Equipment items
    /// </summary>
    /// <param name="selectedEqipment"></param>
    public void DisplaySelectedItem(EquipmentObject selectedEqipment)
    {
        if (equipmentPanel.activeSelf == false)
        {
            equipmentPanel.SetActive(true);
        }

        if (currentItem == selectedEqipment)
        {
            return;   
        }

        currentItem = selectedEqipment;

        GetPanelFields();

        SetItemPanel(selectedEqipment);

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
    }

    /// <summary>
    /// Clears the infomation displayed in the panel
    /// </summary>
    public void ClearDisplay()
    {
        if (currentItem != null)
        {
            currentItem = null;
        }

        SetToDefault();

    }

    /// <summary>
    /// Gets where the infomation needs to be in the panel display
    /// </summary>
    void GetPanelFields()
    {
        if (currentItem == null)
        {
            return;
        }

        switch (currentItem.ItemType)
        {
            case ItemType.Equipment:
                {
                    itemImage = equipmentPanel.transform.Find("ItemImage").GetComponent<Image>();

                    nameText = equipmentPanel.transform.Find($"{itemTextPath}/NameText").GetComponent<TMP_Text>();
                    worthText = equipmentPanel.transform.Find($"WorthText").GetComponent<TMP_Text>();
                    descriptionText = equipmentPanel.transform.Find($"{itemTextPath}/DescriptionText").GetComponent<TMP_Text>();

                    damageText = equipmentPanel.transform.Find($"{itemTextPath}/DamageText").GetComponent<TMP_Text>();
                    defeseText = equipmentPanel.transform.Find($"{itemTextPath}/DefenseText").GetComponent<TMP_Text>();
                    attackSpeedText = equipmentPanel.transform.Find($"{itemTextPath}/AttackSpeedText").GetComponent<TMP_Text>();
                    rangeText = equipmentPanel.transform.Find($"{itemTextPath}/RangeText").GetComponent<TMP_Text>();

                    break;
                }
            default:
                {
                    itemImage = itemPanel.transform.Find("ItemImage").GetComponent<Image>();

                    nameText = itemPanel.transform.Find($"{itemTextPath}/NameText").GetComponent<TMP_Text>();
                    worthText = itemPanel.transform.Find($"WorthText").GetComponent<TMP_Text>();
                    descriptionText = itemPanel.transform.Find($"{itemTextPath}/DescriptionText").GetComponent<TMP_Text>();

                    break;
                }
        }

    }

    /// <summary>
    /// Sets the panel for any Item
    /// </summary>
    /// <param name="item"></param>
    void SetItemPanel(ItemObject item)
    {
        BlankText();

        itemImage.sprite = item.GetIcon;

        nameText.text = item.ItemName;

        worthText.text = item.GetWorth.ToString() + " g";

        if (item.GetDescription != "")
        {
            descriptionText.text = item.GetDescription;
        }
    }

    /// <summary>
    /// Resets the panels to inactive
    /// </summary>
    void SetToDefault()
    {
        if (itemPanel.activeSelf == true)
        {
            itemPanel.SetActive(false);
        }

        if (equipmentPanel.activeSelf == true)
        {
            equipmentPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Clears all text
    /// </summary>
    void BlankText()
    {
        nameText.text = "";
        worthText.text = "";
        descriptionText.text = "";

        if (currentItem.ItemType == ItemType.Equipment)
        {
            damageText.text = "";
            defeseText.text = "";
            attackSpeedText.text = "";
            rangeText.text = "";
        }
    }
}
