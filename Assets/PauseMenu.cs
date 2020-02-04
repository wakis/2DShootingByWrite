using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    gameObj objRule;
    float stack_GradationTimePer;
    int n = 0;
    private void Awake()
    {
        //gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        objRule=Camera.main.GetComponent<gameObj>();
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!objRule.onMenu)
        {
            stack_GradationTimePer = objRule.gradationTimePer;
            objRule.gradationTimePer = 0f;
            if (n > 0)
            {

            }
            else
            {
                n++;
            }
            objRule.onMenu = !objRule.onMenu;
        }
    }

    public void select()
    {
        if (n < 1)
        {
            objRule.gradationTimePer = stack_GradationTimePer;
        }
        if (objRule.onMenu)
        {
            objRule.onMenu = !objRule.onMenu;
        }
        gameObject.SetActive(false);
    }
}
