using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItemManager : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text amountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Image ItemImage { get { return itemImage; } set { itemImage.sprite = value.sprite; } }
    public TMP_Text AmountText { get { return amountText; } set { amountText.text = value.text; } }
}
