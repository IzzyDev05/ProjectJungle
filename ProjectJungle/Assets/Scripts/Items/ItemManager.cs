using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] EquipmentObject eqipmentObjectScript;
    [SerializeField] FoodObject foodObjectScript;
    [SerializeField] MobDropObject mobDropObjectScript;

    public EquipmentObject GetEquipment { get { return eqipmentObjectScript; } }
    public FoodObject GetFood { get { return foodObjectScript; } }
    public MobDropObject GetMobDrop { get { return mobDropObjectScript; } }


}
