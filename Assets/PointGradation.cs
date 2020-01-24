using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGradation : MonoBehaviour
{
    Color defCol = new Color();
    bool colAlphaToOne;
    public bool onTriggerInObj;
    private void Awake()
    {
        defCol = GetComponent<SpriteRenderer>().color;
        onTriggerInObj = false;
        colAlphaToOne = false;
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
            defCol.a += Time.deltaTime * 0.8f;
            if (defCol.a>=1f)
            {
                defCol.a = 1f;
                colAlphaToOne = !colAlphaToOne;
            }
        }
        else
        {
            defCol.a -= Time.deltaTime * 0.8f;
            if (defCol.a <= 0f)
            {
                defCol.a = 0f;
                colAlphaToOne = !colAlphaToOne;
            }
        }
        if (GetComponent<SpriteRenderer>().color != defCol){
            GetComponent<SpriteRenderer>().color = defCol;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!onTriggerInObj)
        {
            if (collision.tag == "Player")
            {
                onTriggerInObj = !onTriggerInObj;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (onTriggerInObj)
        {
            if (collision.tag == "Player")
            {
                onTriggerInObj = !onTriggerInObj;
            }
        }
    }
}
