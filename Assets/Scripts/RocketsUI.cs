using System;
using TMPro;
using UnityEngine;

public class RocketsUI : Singleton<RocketsUI>
{

    public int rocketsOnStart = 0;
    private int RocketsCount = 0;

    public int Rockets => RocketsCount;

    public TMP_Text text;

    private void Start()
    {
       AddRockets(rocketsOnStart);
    }

    public void AddRockets(int i)
    {
        bool ch = RocketsCount <= 0;
        
        RocketsCount += i;

        RocketLaunchpad.Instance.hasAmmo = RocketsCount > 0;
        
        if(ch) RocketLaunchpad.Instance.CanAttack();
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        text.text = RocketsCount.ToString();
        
        
    }
}
