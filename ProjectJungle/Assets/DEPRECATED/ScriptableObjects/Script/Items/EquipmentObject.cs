using UnityEngine;
[CreateAssetMenu(fileName = "New Equipement Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public EquipmentType equipmentType;
    public int damage;
    public int defense;
    public float attackSpeed;
    public float range;

    public void Awake()
    {
        ItemType = ItemType.Equipment;

    }

    /// <summary>
    /// Return the damage the equipment does
    /// </summary>
    public int GetDamage { get { return damage; } }
    /// <summary>
    /// Returns the defense the equipment gives
    /// </summary>
    public int GetDefense { get { return defense; } }
    /// <summary>
    /// Returns the speed the equipment attacks with
    /// </summary>
    public float GetAttackSpeed { get { return attackSpeed; } }
    /// <summary>
    /// Returns the range the equipment can hit
    /// </summary>
    public float GetRange { get { return range; } }
}
