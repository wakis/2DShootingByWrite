using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug_OverLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray))
            Debug.Log(Input.mousePosition);
    }
}
