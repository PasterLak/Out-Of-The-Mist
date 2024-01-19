
public class LootRocket : LootBase
{
    private void Start()
    {
        OnPickUp += DoAfterPickUp;
    }

    protected override void DoAfterPickUp()
    {
        RocketsUI.Instance.AddRockets(Count);
    }
}