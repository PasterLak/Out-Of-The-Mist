public class LootMine : LootBase
{
    private void Start()
    {
        OnPickUp += DoAfterPickUp;
    }

    protected override void DoAfterPickUp()
    {
        MineUI.Instance.AddMines(Count);
        if(Enemy.DropLevel >= 3)
            MineUI.Instance.AddMines(1);
    }
}
