using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public enum PanelPosition
{
    Left,
    Center,
    Right
}

public class ItemPanelManager : MonoBehaviour
{
    public static ItemPanelManager Instance;

    [SerializeField] GameObject itemPanel;

    [SerializeField] Image itemImage;

    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text defeseText;
    [SerializeField] TMP_Text attackSpeedText;
    [SerializeField] TMP_Text rangeText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] ItemObject currentItem;

    [SerializeField] PanelPosition panelPosition;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        panelPosition = PanelPosition.Center;

        SetToDefault();
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

    public void DisplaySelectedItem(FoodObject selectedFood)
    {
        if (currentItem != selectedFood)
        {

        }
    }

    public void DisplaySelectedItem(EquipmentObject selectedEqipment, PanelPosition displayPosition)
    {
        if (itemPanel.activeSelf == false)
        {
            itemPanel.SetActive(true);
        }

        panelPosition = displayPosition;
        ChangePanelPosition();

        if (currentItem == selectedEqipment)
        {
            return;   
        }

        itemImage.sprite = selectedEqipment.GetIcon;

        currentItem = selectedEqipment;

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

        if (selectedEqipment.GetDescription == string.Empty)
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

    void ChangePanelPosition()
    {
        switch (panelPosition)
        {
            case PanelPosition.Left:
                {
                    itemPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0,0);
                    itemPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0,1);
                    itemPanel.GetComponent<RectTransform>().pivot = new Vector2(1,0.5f);

                    break;
                }
            case PanelPosition.Center:
                {
                    itemPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
                    itemPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                    itemPanel.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                    break;
                }
            case PanelPosition.Right:
                {
                    itemPanel.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
                    itemPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                    itemPanel.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                    break;
                }
        }

    }

    void SetToDefault()
    {
        if (itemPanel.activeSelf == true)
        {
            itemPanel.SetActive(false);
        }

        if (itemImage != null)
        {
            itemImage = null;
        }

        damageText.text = "";
        defeseText.text = "";
        attackSpeedText.text = "";
        rangeText.text = "";
        descriptionText.text = "";
    }
}
