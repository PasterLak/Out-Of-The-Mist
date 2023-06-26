using DefaultNamespace;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHasStats
{
    public static int DropLevel = 1;
    public static int ExpLevel = 0;
    public NavMeshAgent agent;
    public int hp;
    public int damage;
    
    private Transform flag;
    private EntityStats Stats;
    private WavesController wc;
    public Animator animator;
    public DamageTag tag;
    public HpTag tagHp;

    public GameObject damagedBody;

    public bool boss = false;

    public bool attackPlayer;
    public void Init(Transform flag, WavesController wc)
    {
        this.flag = flag;
        this.wc = wc;
        SetTarget(flag.position);
        agent.avoidancePriority = Random.Range(0, 50);

        if(boss)
        hp = hp * (WavesController.Instance.GetWave()/10);
      

        Stats = new EntityStats(hp +  wc.waveNumber * 2, damage , 2, 3, ClassType.Attacker);
    }
    void Start()
    {
        int x = Random.Range(0, 101);

        if (x > 60) attackPlayer = true;

        if (Stats.Health >= 10000)
        {
            transform.position = transform.position * 2;
        }
        
        tagHp.UpdateUI(Stats.Health, Stats.MaxHealth);
    }

   
    private void Update()
    {
        if (attackPlayer)
        {
            SetTarget(Player.Instance.transform.position);
        }
    }

    public void SetTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }
    
    public void SetDamage(int damage)
    {
       if(Stats == null) return;
       tag.ShowDamage(damage);
       if (!tagHp.Founded) tagHp.Founded = true;
        Stats.Health -= damage;
        tagHp.UpdateUI(Stats.Health, Stats.MaxHealth);
        
        if(ExpLevel != 0)
        Player.Instance.AddExp(5 * ExpLevel);
        
        if (Stats.Health > 0)
        {
            attackPlayer = true;
        }

        if (Stats.Health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        if (WavesController.Instance.GetWave() % 10 == 0)
        {
            if (boss)
            {
                Player.Instance.AddLevel();
            }
        }
        int x = Random.Range(0, 101) + DropLevel;
        
        if (x == 11)
        {
            if (DropLevel <= 3)
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootRocketSmall"),
                    transform.position, Quaternion.identity);
            }
            else
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootMine"),
                    transform.position, Quaternion.identity);
            }
        }
        if (x == 12)
        {
            if (DropLevel >= 2)
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootRocket"),
                    transform.position, Quaternion.identity);
            }
            else
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootMine"),
                    transform.position, Quaternion.identity);
            }
        }
        if (x > 12 && x <= 14)
        {
            if (DropLevel > 3)
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootAmmo"),
                    transform.position, Quaternion.identity);
                ga.GetComponent<Loot>().count += 3*DropLevel;
            }
        }
        if (x > 14 && x <= 16)
        {
            if (DropLevel == 3)
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootHp"),
                    transform.position, Quaternion.identity);
            }
        }
        
        if (x > 16 && x <= 18)
        {
            if (DropLevel == 2)
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootAmmo"),
                    transform.position, Quaternion.identity);
            }
        }

        if (x > 18 && x <= 22)
        {
         
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootExp"),
                    transform.position, Quaternion.identity);
          
           
        }
        if (x > 22 && x <= 30)
        {
            if (Player.Instance.Stats.Health < 10)
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootHp"),
                    transform.position, Quaternion.identity);
            }

        }
        if (x > 30 && x <= 62)
        {
            if (Player.Instance.bullets < 5)
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootAmmo2"),
                    transform.position, Quaternion.identity);
            }

        }
        if (x > 62 && x <= 72)
        {
        
            if (Player.Instance.bullets < 15 )
            {
                GameObject ga = Instantiate(Resources.Load<GameObject>("LootAmmo"),
                    transform.position, Quaternion.identity);
            }
            
           
        }

        if (x > 72 && x <= 75)
        {
            GameObject ga = Instantiate(Resources.Load<GameObject>("LootRocket"),
                transform.position, Quaternion.identity);
            
        }
       
        if (x > 75 && x <= 82)
        {
            GameObject ga = Instantiate(Resources.Load<GameObject>("LootHp"),
                transform.position, Quaternion.identity);
            
        }
        if (x > 82 && x <= 84)
        {
            GameObject ga = Instantiate(Resources.Load<GameObject>("LootMine"),
                transform.position, Quaternion.identity);
            
        }
        if (x > 84 && x <= 88)
        {
            GameObject ga = Instantiate(Resources.Load<GameObject>("LootAmmo"),
                transform.position, Quaternion.identity);
            
        }
        if(x > 96 && x<= 98)
        {
            GameObject ga = Instantiate(Resources.Load<GameObject>("LootHp2"),
                transform.position, Quaternion.identity);
        }
        if(x > 98)
        {
            GameObject ga = Instantiate(Resources.Load<GameObject>("LootAmmo2"),
                transform.position, Quaternion.identity);
        }
        GameObject ga2 = Instantiate(Resources.Load<GameObject>("kill"), 
            transform.position, Quaternion.identity);
        
        float dis = Vector3.Distance(transform.position, RocketsUI.Instance.transform.position);
        ga2.GetComponent<AudioSource>().volume -= dis * 0.02f;
        
        Player.Instance.AddExp(Stats.Damage  + (int)(ExpLevel * Stats.Damage * 0.5f));
        
        tag.Kill();
        tagHp.Kill();
        if (damagedBody)
        {
            damagedBody.transform.SetParent(null);
            damagedBody.SetActive(true);
        }
       
        wc.RemoveEnemy(this);
       
    }


    public EntityStats GetStats()
    {
        return Stats;
    }
}
