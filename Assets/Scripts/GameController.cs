using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Transform playerTransform;

    public Transform mapCenter;

    public GameObject outsideOfMapMessage;
    public TMP_Text outsideOfMapText;
    public float maxTimeOutside = 5;
    public float maxDistance = 40;
    private float _time;
    private bool active = true;

    private void Awake()
    {
        GameInput.Input = new InputData();
        GameInput.Input.Enable();
    }

    private void Start()
    {
                
        Cursor.visible = false;
       
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
    if(!active) return;
    
        float dis = Distance.Space3DSquared(playerTransform.position, mapCenter.position);

        if (dis > maxDistance * maxDistance)
        {
            outsideOfMapMessage.SetActive(true);
            _time -= Time.deltaTime;

            if (_time < 0)
            {
                _time = 0;
            }
           outsideOfMapText.text = WaveUI.ConvertTimeToMMSS(_time) + "";

           if (_time == 0)
           {
               active = false;
               outsideOfMapMessage.SetActive(false);
               Flag.Instance.Lose();
           }

        }
        else
        {
            _time = maxTimeOutside;
            outsideOfMapMessage.SetActive(false);
        }
    }
}
