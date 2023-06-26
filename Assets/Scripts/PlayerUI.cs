using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarterAssets
{
    public class PlayerUI : MonoBehaviour
    {
        public TMP_Text HpText;
        public Healthbar hpBar;
        public TMP_Text ExpText;
        public TMP_Text LevelText;
        public Slider ExpSlider;
        public TMP_Text BulletsText;

        public Player player;

        private void Start()
        {
            Player.OnHpChanged += ChangeHp;
            Player.OnExpChanged += ChangeExp;
            Player.OnLevelChanged += ChangeLevel;
            Player.OnShooted += ChangeBullets;
            
            ChangeBullets(player.bullets, 0);
            ChangeLevel(player.gold, 0);
            ChangeExp(player.exp, player.maxExp);
            
         
            ChangeHp(player.Stats.Health, player.Stats.MaxHealth);

        }

        public void ChangeHp(int hp, int max)
        {
            hpBar.UpdateSlider(max);
            hpBar.maximumHealth = max;
            hpBar.lowHealth = (int)(max / 3);
            hpBar.highHealth = max - hpBar.lowHealth ;
            hpBar.SetHealth(hp);
            HpText.text = "" + hp + "/" + max;
        }
        
        public void ChangeLevel(int hp, int max)
        {
            
            LevelText.text = hp.ToString();
        }
        public void ChangeExp(int hp, int max)
        {
            ExpSlider.maxValue = max;
            ExpSlider.value = hp;
           
            ExpText.text = "EXP " + hp + "/" + max;
        }

        public void ChangeBullets(int hp, int max)
        {
            BulletsText.text = hp.ToString();
        }
    }
}