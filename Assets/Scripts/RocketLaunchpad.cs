using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class RocketLaunchpad : Singleton<RocketLaunchpad>
{
    public WavesController wc;
    [SerializeField] private Rocket _rocketPrefab;
    private Pool<Rocket> _poolRockets;
    private byte _prefabsCount = 3;
    public float rotationSpeed = 2; 
    public float reloadTime = 5;
    private bool IsReadyToLaunch = false;
    public Rigidbody rigidbody;
    private Rocket _rocketToLaunch = null;
    private Enemy _target = null;
    public Transform spawner;
    public AudioSource audioReload;
    public AudioClip reload;
    public AudioClip findClip;

    public bool hasAmmo = false;

    private float _wait = 5;

    private void Start()
    {
        _poolRockets = new Pool<Rocket>(_rocketPrefab, _prefabsCount, null)
        {
            autoExpand = true
        };

        if (hasAmmo)
            //StartCoroutine(Wait(5));
            _wait = 5;
    }

    IEnumerator Wait(float t)
    {
       
        yield return new WaitForSeconds(t);
        FindTarget();
    }

    private void FindTarget()
    {
    if(_target != null) return;

        audioReload.PlayOneShot(reload);
        if (wc.enemies.Count > 1)
        {
          
            _target = wc.enemies[1];
            GetRocketFromPool();
        }
        else
        {
            _wait = 5;
          // StartCoroutine(Wait(3));
        }
     

    }

    private void GetRocketFromPool()
    {
        _rocketToLaunch = _poolRockets.GetFreeElement();
        _rocketToLaunch.transform.position = spawner.position;
        _rocketToLaunch.transform.rotation = spawner.rotation;
        _rocketToLaunch.gameObject.SetActive(true);
        if (_rocketToLaunch != null)
        {
            _rocketToLaunch.ChangeTarget(_target.gameObject);
        }

        if (_rocketToLaunch && _target) IsReadyToLaunch = true;
    }

    private void Update()
    {
        if (hasAmmo)
        {
            _wait -= Time.deltaTime;
        }

        if (_wait <= 0)
        {
            _wait = 5;

            if (hasAmmo)
            {
                FindTarget();
            }

        }
        if (_target)
        {
            Vector3 targetDirection = (_target.transform.position - transform.position).normalized;
            Quaternion rot = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(targetDirection),
                rotationSpeed * Time.fixedDeltaTime);

            //rigidbody.
            GetComponent<Rigidbody>().rotation = rot;

            transform.localEulerAngles = new Vector3(-90, transform.localEulerAngles.y, 0);
            //rigidbody.velocity = transform.forward * speed;
           
        }

        if (IsReadyToLaunch && _target){
            StartCoroutine(Launch());
        }
}

    IEnumerator Launch()
    {
        IsReadyToLaunch = false;
        if (_rocketToLaunch == null)
        {
            _wait = 1;
            //Wait(1);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        
        RocketsUI.Instance.AddRockets(-1);
        _rocketToLaunch.Launch();
        _rocketToLaunch = null;
        _target = null;

        if (RocketsUI.Instance.Rockets > 0)
        {
            audioReload.PlayOneShot(reload);
            _wait = reloadTime;
            //StartCoroutine(Wait(reloadTime));
        }
       
    }

    public void ReturnRocketToPool(Rocket rocket)
    {

    }
    
    public void CanAttack()
    {
        if (hasAmmo)
        {
            audioReload.PlayOneShot(reload);
            _wait = reloadTime;
            //StartCoroutine(Wait(reloadTime));
        }
        
    }


}
