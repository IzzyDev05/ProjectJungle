using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Object", menuName = "Inventory System/Items/Resource")]
public class ResourceObject : ItemObject
{
    public void Awake()
    {
        Type = ItemType.Resource;
    }
}
