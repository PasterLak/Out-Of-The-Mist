using System;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class DustSystem : MonoBehaviour
{

    public bool IsActive { get; set; }
    public float yOffset;
    [SerializeField] private short updateTime = 500;
    [SerializeField] private  sbyte min = -30;
    [SerializeField] private  sbyte max = 30;

    public Transform playerTransform;
    private Transform particlesPos;
    private short _time;

    private void Start()
    {
       Activate();
       //DontDestroyOnLoad(this);
    }

    public void Activate()
    {
        particlesPos = this.transform;
        _time = updateTime;

   

        IsActive = true;
    }

    private void Update()
    {
        if (!IsActive) return;

        if (_time > 0) _time -= (short)(Time.deltaTime * 1000);
        

        if (_time <= 0)
        {
            sbyte randomX = (sbyte)Random.Range(min, max);
            sbyte randomY = (sbyte)Random.Range(0, 20) ;
            sbyte randomZ = (sbyte)Random.Range(min, max / 2);

            particlesPos.position = new Vector3
            (
                playerTransform.localPosition.x + randomX,
                playerTransform.position.y + randomY + yOffset,
                playerTransform.localPosition.z + randomZ
            );

            _time = updateTime;

        }
    }
}
