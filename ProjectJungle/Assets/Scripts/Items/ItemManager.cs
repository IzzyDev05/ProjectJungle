using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemObject itemObject;
    [SerializeField] int pickupAmount = 1;

    public ItemManager PickupItem()
    {
        AudioManager.instance.PlayOneShot(FModEvents.instance.pickupItem, transform.position);

        gameObject.SetActive(false);

        return this;
    }

    /// <summary>
    /// Returns the parent scriptable object for every item
    /// </summary>
    public ItemObject GetItemObject { get { return itemObject; } }

    public T GetChildItem<T>() where T : ItemObject
    {
        if (itemObject is T)
        {
            return itemObject as T;
        }
        else
        {
            Debug.LogWarning($"{itemObject} is not of the requested type.");
            return null;
        }
    }

    /// <summary>
    /// Returns the number of items to pick up
    /// </summary>
    public int GetAmountPickedUp { get { return pickupAmount; } }
}

