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
        Instance = this;

        SetToDefault();
    }

    /*/ Start is called before the first frame update
    void Start()
    {
        
    }*/

    /*/ Update is called once per frame
    void Update()
    {

    }*/


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

    /*public void DisplaySelectedItem(FoodObject selectedFood)
    {
        if (itemPanel.activeSelf == false)
        {
            itemPanel.SetActive(true);
        }

        if (currentItem == selectedFood)
        {
            return;
        }

        currentItem = selectedFood;

        GetPanelFields();

        SetItemPanel(selectedFood);
    }*/

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

    /*public void DisplaySelectedItem(MobDropObject selectedMobDrop)
    {
        if (itemPanel.activeSelf == false)
        {
            itemPanel.SetActive(true);
        }

        if (currentItem == selectedMobDrop)
        {
            return;
        }

        currentItem = selectedMobDrop;

        GetPanelFields();

        SetItemPanel(selectedMobDrop);
    }*/

    /*public void DisplaySelectedItem(ResourceObject selectedResource)
    {
        if (itemPanel.activeSelf == false)
        {
            itemPanel.SetActive(true);
        }

        if (currentItem == selectedResource)
        {
            return;
        }

        currentItem = selectedResource;

        GetPanelFields();

        SetItemPanel(selectedResource);
    }*/

    public void ClearDisplay()
    {
        if (currentItem != null)
        {
            currentItem = null;
        }

        SetToDefault();

    }

    void GetPanelFields()
    {
        if (currentItem == null)
        {
            return;
        }

        switch (currentItem.Type)
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

    void SetItemPanel(ItemObject item)
    {
        BlankText();

        itemImage.sprite = item.GetIcon;

        nameText.text = item.GetItemName;

        worthText.text = item.GetWorth.ToString() + " g";

        if (item.GetDescription != "")
        {
            descriptionText.text = item.GetDescription;
        }
    }

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

    void BlankText()
    {
        nameText.text = "";
        worthText.text = "";
        descriptionText.text = "";

        if (currentItem.Type == ItemType.Equipment)
        {
            damageText.text = "";
            defeseText.text = "";
            attackSpeedText.text = "";
            rangeText.text = "";
        }
    }
}
