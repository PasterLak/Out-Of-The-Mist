using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats
{
  public int MaxHealth;
  public int Health;
  public int Damage;
  public int Armor;
  public int Speed;
  public ClassType Type { get; private set; }

  public EntityStats(int max, int damage, int armor, int speed, ClassType type)
  {
    MaxHealth = max;
    Health = MaxHealth;

    
    Damage = damage;
    Armor = armor;
    Speed = speed;
    Type = type;
  }
}

public interface IHasStats
{
  public EntityStats GetStats();
}
