using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Button button;
 
    private void Start()
    {
        button.Select();
        button.onClick.AddListener(() => Loader.Load(Loader.Scene.Game));
        
    }

    
    private void Update()
    {
        
    }
}
