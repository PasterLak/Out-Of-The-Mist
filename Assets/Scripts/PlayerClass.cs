using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{

    public static EntityStats GetStats(ClassType type)
    {
        switch (type)
        {
            case ClassType.Tank :

                return new EntityStats(140,10,10,6, type);
            
            case ClassType.Attacker :

                return new EntityStats(25,10,8,7, type);
            
            case ClassType.Shooter :

                return new EntityStats(80,5,5,8, type);;
        }

        return null;
    }
   
}

public enum ClassType
{
    Tank,
    Attacker,
    Shooter
}
