using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TrinketManager : MonoBehaviour
{
    [SerializeField] string Name;
    [SerializeField] GameObject TrinketPrefab;


    #region Inventory_Transform
    [SerializeField] float scale = 1.0f;

    [Header("Inventroy Slot Transform:")]
    [SerializeField] Vector3 s_rotation;

    [SerializeField] Vector3 s_position;

    [Header("Item Display Transform:")]
    [SerializeField] Vector3 d_rotation;

    [SerializeField] Vector3 d_position;
    #endregion

    [TextArea (15,20)]
    [SerializeField] string lore;

    public string TrinketName { get { return Name; } }
    public GameObject TrinketIcon { get { return TrinketPrefab; } }

    public string TricketLore { get { return lore; } }

    public float ScaleMultiplier { get { return scale; } }

    #region Slot_Transform
    public Quaternion S_RotationMultiplier { get { return Quaternion.Euler(s_rotation); } }

    public Vector3 S_PositionModifier { get { return s_position; } }
    #endregion

    #region Display_Transform
    public Quaternion D_RotationMultipler { get { return Quaternion.Euler(d_rotation); } }

    public Vector3 D_PositionModifier { get { return d_position; } }
    #endregion

    public Animator TrinketAnimator { get {  return GetComponent<Animator>(); } }
}
