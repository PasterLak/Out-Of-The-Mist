
using UnityEngine;

public abstract class LootBase : MonoBehaviour
{
   [SerializeField] private float timeToDestroy = 30;
   [SerializeField] private bool destroable = true;
   [SerializeField] private int count;
   
   public int Count
   {
      get => count;
      set => count = value;
   }

   protected delegate void LootAction();
   protected LootAction OnPickUp;

   

   private void Update()
   {
      if(!destroable) return;
      ProcessDestroy();
   }

   private void ProcessDestroy()
   {
    
      timeToDestroy -= Time.deltaTime;

      if (timeToDestroy <= 0)
      {
         Destroy(this.gameObject);
      }
   }

   public void OnTriggerEnter(Collider other)
   {
      if (other.tag != "Player") return;
      
      OnPickUp?.Invoke();
      SpawnPickUpEffect();
   }

   protected abstract void DoAfterPickUp();

   private void SpawnPickUpEffect()
   {
      var ga2 = Instantiate(Resources.Load<GameObject>("add"), 
         transform.position, Quaternion.identity);
            
      Destroy(this.gameObject);
   }

}
