using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mine : MonoBehaviour
{
   public int damage;
   public float radius;
   
   public static int damageBonus = 1;
   
   public void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Enemy")
      {
         GameObject ga = Instantiate(Resources.Load<GameObject>("MineSound"));
         ga.transform.position = transform.position;

         int dm = damageBonus * Random.Range(damage, (int)(damage + damage * 0.2f));
         Enemy en = other.GetComponent<Enemy>();
         
         en.SetDamage(dm);
         
         Collider[] c = Physics.OverlapSphere(transform.position, radius);
         if (c != null && c.Length > 0)
         {
            for (int i = 0; i < c.Length; i++)
            {
               if (c[i].TryGetComponent<Enemy>(out Enemy e))
               {
                  if(e == en) continue;
                  int val = Random.Range(0, dm / 20 + damageBonus);
                  e.SetDamage(val);
               }
            }
         }
         
         Destroy(this.gameObject);
      }
   }
}
