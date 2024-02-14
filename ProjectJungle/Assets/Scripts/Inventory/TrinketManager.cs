using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketManager : MonoBehaviour
{
    [SerializeField] GameObject TrinketPrefab;

    [SerializeField] float scale = 1.0f;

    [SerializeField] Vector3 rotation;

    [TextArea (15,20)]
    [SerializeField] string lore;

    public GameObject TrinketIcon { get { return TrinketPrefab; } }

    public string TricketLore { get { return lore; } }

    public float ScaleMultiplier { get { return scale; } }

    public Quaternion RotationMultiplier { get { return Quaternion.Euler(rotation); } }
}
