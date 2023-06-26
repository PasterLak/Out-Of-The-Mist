using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float gravity;
 
    public float lifeTime;
    public AudioSource exp1;
    public AudioSource exp2;
    private RaycastHit hit;
    public Rigidbody rb;
    private Transform spawnPoint;

    private bool started = false;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        DestroyIt();
    }

    public void AddForce(Transform s)
    {
        spawnPoint = s;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height/2, 0));
        transform.LookAt(ray.GetPoint(50));
        started = true;
        
        rb.AddRelativeForce(transform.forward * speed);
    }

    private void FixedUpdate()
    {
        if(!started) return;
        //rb.velocity = transform.TransformDirection(new Vector3(speed, 0.0f, 0.0f));
        //rb.AddRelativeForce(Vector3.forward * speed);
       // transform.Translate(0,0 , 1);
        //rb.velocity = spawnPoint.forward * speed;
        //rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);

    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
            return;
        if (collision.transform.tag == "Enemy")
        {
            if (collision.transform.TryGetComponent<Enemy>(out Enemy e))
            {
                int pr = Player.Instance.Stats.Damage / 4;
                e.SetDamage(Player.Instance.Stats.Damage + Random.Range(-pr,pr));
               
            }
        }
        DestroyIt();
    }

    private void DestroyIt()
    {
        float dis = Vector3.Distance(transform.position, spawnPoint.position);
        
     
        if(dis <= 20)
        {
            GameObject ga1 = Instantiate(Resources.Load<GameObject>("audioExp1"), 
                            transform.position, Quaternion.identity);
        }
        if(dis > 20 && dis <= 40)
        {
            GameObject ga2 = Instantiate(Resources.Load<GameObject>("audioExp2"), 
                transform.position, Quaternion.identity);
            

        }
        if(dis > 40)
        {
            GameObject ga3 = Instantiate(Resources.Load<GameObject>("audioExp3"), 
                transform.position, Quaternion.identity);
            

        }
        GameObject ga = Instantiate(Resources.Load<GameObject>("explosion"),transform.position,
            Quaternion.identity);
        //gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
