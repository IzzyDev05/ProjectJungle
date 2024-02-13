using System;
using UnityEngine;

public enum Tags
{
    MovingPlatform,
}

public class TagManager : MonoBehaviour
{
    [SerializeField] private Tags tag;

    public Tags ReturnTags()
    {
        return tag;
    }
}