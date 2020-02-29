using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopCredit : MonoBehaviour
{
    [SerializeField]
    GameObject Credit;
    [SerializeField]
    Text pButton;
    [SerializeField, Multiline(2)]
    string[] OpCl = new string[2];
    private void Awake()
    {
        Credit.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Credit.SetActive(!Credit.activeSelf);
            if (Credit.activeSelf)
            {
                if (pButton.text != OpCl[1])
                    pButton.text = OpCl[1];
            }
            else
            {
                if (pButton.text != OpCl[0])
                    pButton.text = OpCl[0];
            }

        }
    }
}
