using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Loot : MonoBehaviour
{
    // Start is called before the first frame update
    public LootType type;
    public int count;
    public float timeToDestroy = 30;
    public bool destroable = true;
    
    public void Init(int count)
    {
        
    }

    void Update()
    {
        if(!destroable) return;
        timeToDestroy -= Time.deltaTime;

        if (timeToDestroy <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (type)
            {
               case LootType.Ammo :
                   Player.Instance.AddAmmo(count);
                   break;
               case LootType.Hp :
                   if(Player.Instance.GetStats().Health == Player.Instance.GetStats().MaxHealth) return;
                   Player.Instance.AddHpWithBonus(count);
                   break;
               case LootType.Exp :
                   Player.Instance.AddExp(count * Enemy.DropLevel);
                   if(Enemy.DropLevel >= 5)
                       Player.Instance.AddExp(count * Enemy.DropLevel * 5);
                       
                   break;
               case LootType.Rocket :
                   RocketsUI.Instance.AddRockets(count);
                       
                   break;
               case LootType.Mine :
                   MineUI.Instance.AddMines(count);
                   if(Enemy.DropLevel >= 3)
                       MineUI.Instance.AddMines(1);
                   break;
            }
            
            GameObject ga2 = Instantiate(Resources.Load<GameObject>("add"), 
                transform.position, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
    }

    public enum LootType
    {
        Hp,
        Ammo,
        Exp,
        Rocket,
        Mine
    }
}
