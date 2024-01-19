using StarterAssets;

public class LootExp : LootBase
{
    private void Start()
    {
        OnPickUp += DoAfterPickUp;
    }

    protected override void DoAfterPickUp()
    {
        Player.Instance.AddExp(Count * Enemy.DropLevel);
        if(Enemy.DropLevel >= 5)
            Player.Instance.AddExp(Count * Enemy.DropLevel * 5);
    }
}
