using UnityEngine;

public class Coin : CollectibleBase
{
    protected override void OnCollected()
    {
        Debug.Log("Collected a Coin!");
    }
}
