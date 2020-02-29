using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setcol : MonoBehaviour
{
    LineRenderer line;
    PolygonCollider2D pol;
    public float setSize;
    private void Awake()
    {
        pol = GetComponent<PolygonCollider2D>();
        line = GetComponent<LineRenderer>();
        List<Vector2> linepos = new List<Vector2>();
        Vector2[] Max_min = { Vector2.zero, Vector2.zero };//0最小1最大
        for (int lp = 0; lp < line.positionCount; lp++)
        {
            linepos.Add(line.GetPosition(lp));
            if (Max_min[0].x > line.GetPosition(lp).x)
                Max_min[0].x = line.GetPosition(lp).x;
            if (Max_min[0].y > line.GetPosition(lp).y)
                Max_min[0].y = line.GetPosition(lp).y;
            if (Max_min[1].x < line.GetPosition(lp).x)
                Max_min[1].x = line.GetPosition(lp).x;
            if (Max_min[1].y < line.GetPosition(lp).y)
                Max_min[1].y = line.GetPosition(lp).y;
        }
        pol.points = linepos.ToArray();
        gameObject.AddComponent<bullet>().setBulletStatus(Max_min[1]- Max_min[0]);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (setSize > 0f)
        {
            var size = transform.localScale;
            size -= Vector3.one * Time.deltaTime/2f;
            transform.localScale = size;
            setSize -= Time.deltaTime / 2f;
        }
        else
        {
            Destroy(this);
        }
    }
}
