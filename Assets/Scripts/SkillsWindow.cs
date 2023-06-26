using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsWindow : MonoBehaviour
{

    public GameObject skillwindow;
    public StarterAssetsInputs input;
    public Color activ;
    public Color noactiv;
    public Button[] buttons;
    public Image[] buttonsIm;

    public TMP_Text[] skillCount;

    public AudioSource click;

    private int[] skillCounter;
    // Start is called before the first frame update
    void Start()
    {
        skillCounter = new int[skillCount.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (input.skills)
        {
            if (!skillwindow.activeSelf)
            {
                OpenWindow();
                input.skills = false;
                return;
            }
            if (skillwindow.activeSelf)
            {
                CloseWindow();
                input.skills = false;
                return;
            }
           
        }
    }

    public void OpenWindow()
    {
        buttons[0].Select();
        skillwindow.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
        Player.Instance.ACTIVE = false;
        Cursor.lockState = CursorLockMode.None;

        UpdateWindow();
    }

    private void UpdateWindow()
    {
        if (Player.Instance.gold < 1)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = false;
                buttonsIm[i].color = noactiv;
            }
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
                buttonsIm[i].color = activ;
            }
        }
    }

    public void CloseWindow()
    {
        skillwindow.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        Player.Instance.ACTIVE = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Buy2(int id)
    {

        skillCounter[id]++;
        skillCount[id].text = skillCounter[id].ToString();
        
        switch (id)
        {
          case  0:
              Buy(Skill.Damage);
              break;
          case  1:
              Buy(Skill.Hp);
              break;
          case  2:
              Buy(Skill.Speed);
              break;
          case  3:
              Buy(Skill.Jump);
              break;
          case  4:
              Buy(Skill.Drop);
              break;
          case  5:
              Buy(Skill.Ammo);
              break;
          case  6:
              Buy(Skill.RestoreHp);
              break;
          case  7:
              Buy(Skill.Exp);
              break;
          case  8:
              Buy(Skill.Heal);
              break;
        }
    }

    public void Buy(Skill skill)
    {
  
        click.Play();
        Player.Instance.gold--;
        Player .OnLevelChanged?.Invoke(Player.Instance.gold,0);

        switch (skill)
        {
            case Skill.Damage:
                Player.Instance.AddDamage();
                Mine.damageBonus++;
                break;
            case Skill.Hp:
                Player.Instance.AddMaxHp();
                break;
            case Skill.Speed:
                Player.Instance.AddSpeed();
                Player.Instance.AddExp((int)Player.Instance.MoveSpeed * 10);
                break;
            case Skill.Drop:
                Enemy.DropLevel++;
                break;
            case Skill.Ammo:
                Player.Instance.AddAmmo(200);
                break;
            case Skill.RestoreHp:
                Player.Instance.AddHp(500);
                break;
            case Skill.Jump:
                Player.Instance.AddJump();
                Player.Instance.AddExp((int)Player.Instance.JumpHeight * 10);
                break;
            case Skill.Exp:
                Enemy.ExpLevel++;
                break;
            case Skill.Heal:
                Player.Instance.AddHealSpeed();
                break;
        }

        UpdateWindow();
    }

    private void AddDamage()
    {
        
    }
    


    public enum Skill
    {
        Damage = 0,
        Hp = 1,
        Speed = 2,
        Jump  =3 ,
        Drop = 4,
        Ammo = 5,
        RestoreHp = 6,
        Exp = 7,
        Heal = 8
        
    }
}
