using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGrid : MonoBehaviour
{

    public Grid grid;
    

   // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(1, "./Assets/Maps/map1.txt");
       
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }

   
}
