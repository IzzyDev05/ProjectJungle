using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinketManager : MonoBehaviour
{
    [SerializeField] Sprite trinketSprite;

    [TextArea (15,20)]
    [SerializeField] string lore;

    public Sprite TrinketSprite { get { return trinketSprite; } }

    public string TricketLore { get { return lore; } }
}
