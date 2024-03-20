using System;
using UnityEngine;

public enum Tags
{
    MovingPlatform,
}

public class TagManager : MonoBehaviour
{
    [SerializeField] private Tags _tag;

    public Tags ReturnTags()
    {
        return _tag;
    }
}