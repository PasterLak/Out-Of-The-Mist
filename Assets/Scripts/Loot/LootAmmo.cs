using StarterAssets;
using UnityEngine;

public class LootAmmo : LootBase
{
    private void Start()
    {
        OnPickUp += DoAfterPickUp;
    }

    protected override void DoAfterPickUp()
    {
        Player.Instance.AddAmmo(Count);

        Debug.Log("Added");
    }
}
