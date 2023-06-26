using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class WavesController : Singleton<WavesController>
    {
        
        
        public byte waveNumber = 0;
        private float timeBeforeAttack = 5;
        public GameObject prefabEnemy;
        public Transform[] spawners;
        public Transform flag;
        public Player Player;
        
        public delegate void MyEvent();
        public delegate void Timed(float time, float maxTime);
        public delegate void DEnemies(int current,int max);

        public static Timed OnTimeUpdated;
        public static MyEvent OnWaveStarted;
        public static DEnemies OnEnemiesUpdated;
        public static MyEvent OnWaveEnded;

        public AudioSource waveSound;

        private float _currentTime;

        public List<Enemy> enemies = new List<Enemy>();
        private int maxEnemies = 6;

        private void Start()
        {
            _currentTime = 5;
            timeBeforeAttack = 10;
        }

        public int GetWave()
        {
            return waveNumber;
        }

        private void Update()
        {

            if (_currentTime > 0)
            {
                UpdateTime();
            }
            if(_currentTime < 0)
            {
                _currentTime = 0;
                
                OnTimeUpdated?.Invoke(_currentTime, timeBeforeAttack);
                StartWave();
            }

            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    float dis = Distance.Space3D(enemies[i].transform.position, Player.GetCenter());

                    if (dis < 1.6f)
                    {
                        this.Player.SetDamage(enemies[i].GetStats().Damage);
                        enemies[i].Kill();
                     
                        
                    }
                }
                
            }
        
        }

        public void RemoveEnemy(Enemy e)
        {
            enemies.Remove(e);
            Flag.Instance.RemoveEnemy(e);
            Destroy(e.gameObject);
            OnEnemiesUpdated?.Invoke(enemies.Count,maxEnemies);
            if (enemies.Count == 0)
            {
               
                EndWave();
            }
        }

        private void StartWave()
        {
            waveNumber++;
            OnWaveStarted?.Invoke();
            waveSound.Play();
            SpawnEnemies();
            OnEnemiesUpdated?.Invoke(enemies.Count,maxEnemies);
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                Vector3 spawnPos = spawners[(int)Random.Range(0,spawners.Length)].position;
                
                if (waveNumber   == 1)
                {
                    if (i == 0  )
                    {
                        GameObject ga5 = Instantiate(Resources.Load<GameObject>("Enemy6"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga5.GetComponent<Enemy>());
                        ga5.GetComponent<Enemy>().Init(flag, this);
                        continue;
                    }
                }
                
                if (waveNumber  % 2 == 0)
                {
                    if (i == 0 || i == 1)
                    {
                        GameObject ga3 = Instantiate(Resources.Load<GameObject>("Enemy3"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga3.GetComponent<Enemy>());
                        ga3.GetComponent<Enemy>().Init(flag, this);
                        continue;
                    }
                }
                if (waveNumber  % 3 == 0)
                {
                    if (i == 2)
                    {
                        GameObject ga3 = Instantiate(Resources.Load<GameObject>("Enemy6"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga3.GetComponent<Enemy>());
                        ga3.GetComponent<Enemy>().Init(flag, this);
                        continue;
                    }
                }

                if (waveNumber % 5 == 0)
                {
                    if (i == 0)
                    {
                        GameObject ga3 = Instantiate(Resources.Load<GameObject>("Enemy4"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga3.GetComponent<Enemy>());
                        ga3.GetComponent<Enemy>().Init(flag, this);
                        continue;
                    }
                }
                
                if (waveNumber % 10 == 0)
                {
                    if (i == 3)
                    {
                        GameObject ga5 = Instantiate(Resources.Load<GameObject>("Enemy5"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga5.GetComponent<Enemy>());
                        ga5.GetComponent<Enemy>().Init(flag, this);
                        continue;
                    }
                }
                
                if (i < (maxEnemies/3f))
                {
                    if (waveNumber < 10)
                    {
                        GameObject ga1 = Instantiate(Resources.Load<GameObject>("Enemy1"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga1.GetComponent<Enemy>());
                        ga1.GetComponent<Enemy>().Init(flag, this);
                    }
                    if(waveNumber >= 10 && waveNumber < 20)
                    {
                        GameObject ga1 = Instantiate(Resources.Load<GameObject>("Enemy7"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga1.GetComponent<Enemy>());
                        ga1.GetComponent<Enemy>().Init(flag, this);
                    }
                    if(waveNumber >= 20 && waveNumber < 30)
                    {
                        GameObject ga1 = Instantiate(Resources.Load<GameObject>("Enemy8"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga1.GetComponent<Enemy>());
                        ga1.GetComponent<Enemy>().Init(flag, this);
                    }
                    if(waveNumber >= 30 )
                    {
                        GameObject ga1 = Instantiate(Resources.Load<GameObject>("Enemy9"),
                            spawnPos, Quaternion.identity);
                        enemies.Add(ga1.GetComponent<Enemy>());
                        ga1.GetComponent<Enemy>().Init(flag, this);
                    }
                   
                    continue;
                }
                
                GameObject ga = Instantiate(Resources.Load<GameObject>("Enemy2"), spawnPos, Quaternion.identity);
                enemies.Add(ga.GetComponent<Enemy>());
                ga.GetComponent<Enemy>().Init(flag, this);
                
            }
        }
        
        private void EndWave()
        {
            OnWaveEnded?.Invoke();
            
            if(maxEnemies < 40)
            maxEnemies += 2;
            
            _currentTime = timeBeforeAttack;
        }

        private void UpdateTime()
        {
          
            _currentTime -= Time.deltaTime;
            OnTimeUpdated?.Invoke(_currentTime,timeBeforeAttack);
        }
    }
}