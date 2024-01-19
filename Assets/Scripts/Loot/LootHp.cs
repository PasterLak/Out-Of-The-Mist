using StarterAssets;

public class LootHp : LootBase
{
    private void Start()
    {
        OnPickUp += DoAfterPickUp;
    }

    protected override void DoAfterPickUp()
    {
      
        if (Player.Instance.GetStats().Health == Player.Instance.GetStats().MaxHealth) return;
        Player.Instance.AddHpWithBonus(Count);

    }
}