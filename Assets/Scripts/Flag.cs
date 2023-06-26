using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    
    public static Flag Instance;

    public Image image;
    public Button button;
    public GameObject loseWindow;
    public Transform flagPos;
    public float occupated = 0;

    public AudioSource alarm;
    public AudioSource flagOn;

    public List<Enemy> enemiesIn = new List<Enemy>();

    private float startY;

    void Awake()
    {
        Instance = this;
        startY = flagPos.position.y;
    }
    void Start()
    {
        image.gameObject.SetActive(false);
        button.onClick.AddListener(() => OpenMenu());
    }
    
    public void OpenMenu()
    {
        Time.timeScale = 1;
        Loader.Load(Loader.Scene.Menu);
    }

    public void Occupe(float t)
    {
        occupated += t;

        flagPos.position = new Vector3(flagPos.position.x, startY - occupated*3.2f, flagPos.position.z);

        if (occupated > 0)
        {
            image.gameObject.SetActive(true);

        }
        else
        {
            image.gameObject.SetActive(false);
        }

        if (occupated > 1)
        {
            
            occupated = 1;
            Lose();
        }
        
        if (occupated < 0)
        {
            occupated = 0;
            alarm.Stop();
            flagOn.Play();
        }
        
        
        image.fillAmount = occupated;
    }

    IEnumerator PP()
    {
        yield return new WaitForSeconds(4);
        flagOn.Stop();
    }

    public void Lose()
    {
        Time.timeScale = 0;
        alarm.Stop();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        loseWindow.SetActive(true);
    }

    public void OnTriggerEnter(Collider other)
    {
         if (other.tag == "Enemy")
         {
             Enemy e = other.gameObject.GetComponent<Enemy>();
             if (!enemiesIn.Contains(e))
             {
                 enemiesIn.Add(e);
             }
                    alarm.Play();
                    e.animator.enabled = false;
                    e.agent.ResetPath();
                }
    }

    public void RemoveEnemy(Enemy e)
    {
        if (enemiesIn.Contains(e))
        {
            enemiesIn.Remove(e);

            if (enemiesIn.Count == 0)
            {
                alarm.Stop();
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            RemoveEnemy(e);
            alarm.Stop();
            e.animator.enabled = true;
        }

        if (other.tag == "Player")
        {
            flagOn.Stop();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            alarm.Stop();
            if(occupated > 0)
            Occupe(Time.deltaTime * -0.2f);
        }
        
        if (other.tag == "Enemy")
        {
        
            Occupe(Time.deltaTime * 0.04f);
        }
    }

}
