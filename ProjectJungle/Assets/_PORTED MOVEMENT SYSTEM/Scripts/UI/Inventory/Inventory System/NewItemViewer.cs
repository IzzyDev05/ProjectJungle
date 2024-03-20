using UnityEngine;
using TMPro;

public class NewItemViewer : MonoBehaviour
{
    public static NewItemViewer Instance;

    [SerializeField] Transform itemIconParent;
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

    /// <summary>
    /// Opens the item viewer and gets relevant info of the item
    /// </summary>
    /// <param name="itemIcon">The 3D object to display.</param>
    /// <param name="name">Name of the item.</param>
    /// <param name="lore">Lore of the item.</param>
    /// <param name="scale">Scale the item should have.</param>
    /// <param name="rotation">The Rotation the item should have.</param>
    public void OpenItemViewer(TrinketManager trinket)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        Transform item = Instantiate(trinket.TrinketIcon, itemIconParent.transform).transform;
        item.localScale *= trinket.ScaleMultiplier;
        item.localRotation *= trinket.D_RotationMultipler;
        item.localPosition += trinket.D_PositionModifier;
        item.gameObject.layer = LayerMask.NameToLayer("Trinket");
        item.GetComponent<Animator>().enabled = false;

        nameText.text = trinket.TrinketName;
        descriptionText.text = trinket.TricketLore;
    }

    /// <summary>
    /// Hides the item viewer.
    /// </summary>
    public void HideItemViewer()
    {
        if (gameObject.activeSelf == true)
        {
            ClearItemIcon();

            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Clears the 3D objects from the item icon
    /// </summary>
    private void ClearItemIcon()
    {
        foreach (Transform child in itemIconParent)
        {
            Destroy(child.gameObject);
        }
    }
}
