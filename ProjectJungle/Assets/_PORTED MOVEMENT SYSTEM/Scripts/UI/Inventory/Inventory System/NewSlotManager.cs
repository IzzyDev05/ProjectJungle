using UnityEngine;
using UnityEngine.UI;

public class NewSlotManager : MonoBehaviour
{
    [SerializeField] GameObject slotItem;
    [SerializeField] GameObject itemIconParent;
    [SerializeField] Image emptyImage;

    bool isCollected = false; 

    private void Awake()
    {
        if (itemIconParent.activeSelf == true)
        {
            itemIconParent.SetActive(false);
        }

        if (isCollected == true)
        {
            isCollected = false;
        }
    }

    private void Start()
    {
        TrinketManager trinket = slotItem.GetComponent<TrinketManager>();

        Transform item = Instantiate(trinket.TrinketIcon, itemIconParent.transform).transform;
        item.localPosition += trinket.S_PositionModifier;
        item.localScale *= trinket.ScaleMultiplier;
        item.localRotation *= trinket.S_RotationMultiplier;
        item.gameObject.layer = LayerMask.NameToLayer("Trinket");
        item.GetComponent<Animator>().enabled = false;

        this.name = trinket.TrinketName + " Slot";

        SetupButton();
    }

    /// <summary>
    /// Check if the item picked up matches the item for this slot
    /// </summary>
    /// <param name="pickedUpItem">A Game Object of the item that was picked up</param>
    /// <returns>True if the item matched the slot item</returns>
    public bool MatchSlotItem(GameObject pickedUpItem)
    {
        if (slotItem == null)
        {
            //Debug.LogError("Slot Item is NULL");
            return false;
        }

        Debug.Log(slotItem.name.Contains(pickedUpItem.name) ? true : false);
        return slotItem.name.Contains(pickedUpItem.name) ? true : false;
    }

    /// <summary>
    /// Add the item to the slot
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item)
    {
        AudioManager.Instance.PlayOneShot(FModEvents.Instance.pickupItem, GameManager.Player.transform.position);

        itemIconParent.SetActive(true);
        Destroy(item);

        isCollected = itemIconParent.activeSelf;
    }

    #region UI
    /// <summary>
    /// Display the information of the item when click on if the item is collected
    /// </summary>
    public void OpenItemPanel()
    {
        if (isCollected == false)
        {
            return;
        }

        NewItemViewer.Instance.OpenItemViewer(slotItem.GetComponent<TrinketManager>());
    }

    /// <summary>
    /// Set up the slots so the item can be displayed when clicked on
    /// </summary>
    void SetupButton()
    {
        Button button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(OpenItemPanel);
    }
    #endregion

    public GameObject SetSlotItem { set { slotItem = value; } }

    public bool IsCollected { get {  return isCollected; } }
}
