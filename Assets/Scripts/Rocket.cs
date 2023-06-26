using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour
{
    
    
    [Header("References")] [SerializeField]
    private GameObject _target;

    private Rigidbody _targetRigidbody;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private ParticleSystem _rocketFire;
    [SerializeField] private ParticleSystem _rocketStartSteam;
    [SerializeField] private ParticleSystem _rocketSmoke;
    [SerializeField] private AudioSource _rocketSound;
    [SerializeField] private AudioSource _launchSound;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private RocketLaunchpad launchpad;

    [Header("Parametres")] private float _currentSpeed = 0;
    [SerializeField] private float _maxSpeed = 13;
    [SerializeField] private float _rotateSpeed = 95;
    [SerializeField]private float _acceleration = 8f;
    private float _takeOffTime = 2f;
    private float _climbTime = 1.75f;

    [SerializeField]private float _maxDistancePredict = 100;
    [SerializeField]private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [SerializeField]private float _deviationAmount = 100;
    [SerializeField] private float _deviationSpeed = 3;

    private bool IsLaunched = false;
    private bool IsGainedAltitude = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _targetRigidbody = _target.GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    public void Attack(GameObject target)
    {
        if (IsLaunched) return;

        _target = target;
        _targetRigidbody = _target.GetComponent<Rigidbody>();
        Launch();
    }

    public void ChangeTarget(GameObject target)
    {
        _target = target;
        _targetRigidbody = _target.GetComponent<Rigidbody>();
    }

    public void Launch()
    {
        if (IsLaunched) return;

        
        _rocketFire.Play();
        _launchSound?.Play();
        _rocketStartSteam.Play();

        StartCoroutine(LaunchRocket());
    }

    private IEnumerator LaunchRocket()
    {
        if (_target == null) yield break;
        if (_targetRigidbody == null) yield break;

        yield return new WaitForSeconds(_takeOffTime);

        _rocketSound?.Play();
        _rocketSmoke.Play();
        IsLaunched = true;

        StartCoroutine(GainAltitude());
    }

    private IEnumerator GainAltitude()
    {
        yield return new WaitForSeconds(_climbTime);
        IsGainedAltitude = true;
    }

    private void FixedUpdate()
    {
        if (_target == null)
        {
            Explosion();
            return;
        }

        if (!IsLaunched) return;
        UpdateDistance();
        if (_currentSpeed < _maxSpeed)
            _currentSpeed += Time.deltaTime * _acceleration;

        _rigidbody.velocity = transform.forward * _currentSpeed;

        if (!IsGainedAltitude) return;
        if (_target == null)
        {
            Explosion();
            return;
        }
        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict,
            _maxDistancePredict,
            Vector3.Distance(_transform.position, _target.transform.position));

        PredictMovement(leadTimePercentage);

        AddDeviation(leadTimePercentage);

        RotateRocket();
    }

    private void UpdateDistance()
    {
        if(_target == null) return;
        float dis = Distance.Space3D(transform.position, _target.transform.position);

        if (dis < 1)
        {
            Kill();
        }
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = _targetRigidbody.position + _targetRigidbody.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = _transform.TransformDirection(deviation)
                               * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - _transform.position;

        var rotation = Quaternion.LookRotation(heading);

        _rigidbody.MoveRotation(Quaternion.RotateTowards(
            _transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    private void Kill()
    {
        if (!IsLaunched) return;

        Audio();

        if (_explosionPrefab)
            Instantiate(_explosionPrefab,
                _transform.position, Quaternion.identity);

        int dm = Random.Range(400, 600);
            _target.GetComponent<Enemy>().SetDamage(dm);
            _target = null;

            Collider[] c = Physics.OverlapSphere(transform.position, 5f);
            if (c != null && c.Length > 0)
            {
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i].TryGetComponent<Enemy>(out Enemy e))
                    {
                        int val = Random.Range(0, dm / 2);
                        e.SetDamage(val);
                    }
                }
            }
            gameObject.SetActive(false);
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    private void Audio()
    {
        float dis = Vector3.Distance(transform.position, RocketsUI.Instance.transform.position);

        if (dis <= 15)
        {
            GameObject g1 = Instantiate(Resources.Load<GameObject>("rocket1"));
            
        }

        if (dis > 15 && dis <= 25)
        {
            GameObject g2 = Instantiate(Resources.Load<GameObject>("rocket2"));
        }
        if (dis > 25 && dis <= 35)
        {
            GameObject g2 = Instantiate(Resources.Load<GameObject>("rocket3"));
        }
        else
        {
            GameObject g3 = Instantiate(Resources.Load<GameObject>("rocket4"));
        }
    }

    private void Explosion()
    {
        
        Audio();
        if (_explosionPrefab)
            Instantiate(_explosionPrefab,
                _transform.position, Quaternion.identity);
        
        
        int dm = Random.Range(400, 600);
        Collider[] c = Physics.OverlapSphere(transform.position, 8f);
        if (c != null && c.Length > 0)
        {
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i].TryGetComponent<Enemy>(out Enemy e))
                {
                    int val = Random.Range(0, dm / 2);
                    e.SetDamage(val);
                }
            }
        }

       
      
        gameObject.SetActive(false);
    }

}