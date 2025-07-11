using UnityEngine;

public class Star : CollectibleBase
{
    protected override void OnCollected()
    {
        Debug.Log("Collected a Star!");
    }
}
