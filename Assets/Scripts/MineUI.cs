using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineUI : Singleton<MineUI>
{

    public int onStart = 0;
    private int _count = 0;

    public int Count => _count;

    public TMP_Text text;
    public Image icon;
    public AudioSource placeSound;

    private void Start()
    {
        AddMines(onStart);

        GameInput.Input.Player.Mine.performed += _ => PlaceMine();
    }

    public void AddMines(int i)
    {
        bool ch = _count <= 0;
        
        _count += i;


        
        UpdateUI();
    }

    public void PlaceMine()
    {
        if(_count < 1) return;
        
        
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height/2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            placeSound.Play();
            GameObject ga = Instantiate(Resources.Load<GameObject>("Mine"));
            ga.transform.position = hit.point + new Vector3(0,0.1f, 0);
            AddMines(-1);
        }
    }

    public void UpdateUI()
    {
        
        text.text = _count.ToString();

        if (_count <= 0)
        {
            icon.enabled = false;
            text.enabled = false;
        }
        else
        {
            icon.enabled = true;
            text.enabled = true;
        }
        
    }
}