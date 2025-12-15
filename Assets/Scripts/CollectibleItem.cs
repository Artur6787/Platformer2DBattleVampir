using System;
using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    public event Action<CollectibleItem> Collected;

    public virtual void HandleCollected()
    {
        Collected?.Invoke(this);
    }
}