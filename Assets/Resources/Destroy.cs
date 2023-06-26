using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy
    : MonoBehaviour
{
    public float time = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator Kill()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
