using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItemManager : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text amountText;

    [SerializeField] ItemObject itemScript;

    [SerializeField] int amount = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (itemScript != null)
        {
            itemImage.sprite = itemScript.GetIcon;
        }

        if (amount > 1)
        {
            amountText.text = amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
    }

    public Image ItemImage { get { return itemImage; } set { itemImage.sprite = value.sprite; } }
    public TMP_Text AmountText { get { return amountText; } set { amountText.text = value.text; } }

    public ItemObject Item { get { return itemScript; } set { itemScript = value; } }
}
