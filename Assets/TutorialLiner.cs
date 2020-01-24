using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLiner : MonoBehaviour
{
    List<Vector3> linePositions = new List<Vector3>();
    LineRenderer line;
    public Vector3 lineTip;
    int maxLineCount;
    float timer;
    float settime;
    [SerializeField]
    Transform penTip;
    private void Awake()
    {
        linePositions.Clear();
        line = GetComponent<LineRenderer>();
        maxLineCount = line.positionCount;
        for (int lp=0;lp<line.positionCount;lp++)
        {
            linePositions.Add(line.GetPosition(lp));
        }
        settime = 1f / line.positionCount;
        lineTip = new Vector3();
    }
    // Start is called before the first frame update
    void Start()
    {
        line.positionCount = 1;
        line.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        loopLiner();
        penTip.localPosition = lineTip;
    }
    
    void loopLiner()
    {
        timer += Time.deltaTime;
        if(maxLineCount!= line.positionCount)
        {
            if (timer > settime)
            {
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, linePositions[line.positionCount - 1]);
                lineTip = linePositions[line.positionCount - 1];
                timer = 0f;
            }
        }else
        {
            if (timer > settime)
            {
                line.loop = true;
                lineTip = linePositions[0];
            }
            if (timer > 1f&& timer > settime*2f)
            {
                line.positionCount = 1;
                line.loop = false;
                timer = 0f;
            }
        }
    }
}
