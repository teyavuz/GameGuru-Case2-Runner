using UnityEngine;

public class Diamond : CollectibleBase
{
    protected override void OnCollected()
    {
        Debug.Log("Collected a Diamond!");
    }
}
