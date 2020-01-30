using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popping : MonoBehaviour
{
    Color defCol = new Color();
    bool colAlphaToOne;
    public bool onTriggerInObj;
    float timer;
    private void Awake()
    {
        defCol = GetComponent<SpriteRenderer>().color;
        onTriggerInObj = false;
    }
        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (colAlphaToOne)
        {
            defCol.a += Time.deltaTime * 2f;
            if (defCol.a >= 1f)
            {
                timer += Time.deltaTime;
                defCol.a = 1f;
                if (timer > 1f)
                {
                    timer = 0;
                    colAlphaToOne = !colAlphaToOne;
                }
            }
        }
        else
        {
            defCol.a -= Time.deltaTime * 1.8f;
            if (defCol.a <= 0f)
            {
                defCol.a = 0f;
                colAlphaToOne = !colAlphaToOne;
            }
        }
        if (GetComponent<SpriteRenderer>().color != defCol)
        {
            GetComponent<SpriteRenderer>().color = defCol;
        }
    }
}
