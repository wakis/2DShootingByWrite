using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

struct boolVector
{
    public bool boolean;
    public Vector3 vector3;
    public boolVector(bool bl,Vector3 vect)
    {
        this.boolean = bl;
        this.vector3 = vect;
    }
}
public class addLine : MonoBehaviour
{
    player Player;//プレイヤーObject格納
    public float num;
    List<LineRenderer> lineList = new List<LineRenderer>();//線のlist

    public Material lineMaterial;//線のMaterial

    public Color stlineColor; //線の色
    public Color edlineColor;

    public float lineWidth=0.1f;//線の太さ

    Vector3[] pointAngle = new Vector3[3];

    public int lineLimit = 100;

    public Material test;
    public SpriteRenderer filter;
    [SerializeField]
    Color ConcentCol;
    [SerializeField]
    Color HitCol;
    Color setCol;
    public float MaxHP;
    [System.NonSerialized]
    public float HP;

    gameObj objRule;
    
    public bool canAddLine;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        canAddLine = true;
        objRule = Camera.main.GetComponent<gameObj>();
        Player = objRule.player.GetComponent<player>();
        Concentration();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(canAddLine == objRule.onMenu)
        {
            canAddLine = !canAddLine;
        }*/
        Concentration();
        if (Input.GetMouseButtonDown(0)&&(canAddLine&&!objRule.onMenu))
        {
            objRule.onConcentration = true;
            AddLine_Obj();
        }
        if (Input.GetMouseButton(0) && (canAddLine && !objRule.onMenu))
        {
            AddPositionToLineRend();
        }
        if ((Input.GetMouseButtonUp(0)&& objRule.onConcentration) || (objRule.onConcentration&&!(canAddLine && !objRule.onMenu)))
        {
            filter.transform.position = Vector3.zero;
            objRule.onConcentration = false;
            if (lineList.Last().positionCount < 3)//要素数不足
            {
                Destroy(lineList.Last().gameObject);
                lineList.Remove(lineList.Last());
            }
            else
            {
                makeLineToObj();
                Player.addPlayerBullet(lineList.Last().gameObject);
            }
        }
    }

    void AddLine_Obj()//線オブジェクトの生成、初期化
    {
        //線オブジェクト生成、をリストに追加
        GameObject lineObj = new GameObject();
        lineObj.transform.parent = Camera.main.transform;
        lineObj.AddComponent<LineRenderer>();
        lineList.Add(lineObj.GetComponent<LineRenderer>());

        //線オブジェクトの初期化
        lineList.Last().positionCount = 0;
        lineList.Last().material = lineMaterial;
        lineList.Last().material.color = stlineColor;
        lineList.Last().startColor= stlineColor;
        lineList.Last().endColor = edlineColor;
        lineList.Last().startWidth = lineWidth;
        lineList.Last().endWidth = lineWidth;
    }

    void AddPositionToLineRend()//クリックドラック中
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 10.0f;
        if (lineList.Last().positionCount == 0)
        {
            lineList.Last().positionCount++;
            lineList.Last().SetPosition(lineList.Last().positionCount - 1, Camera.main.ScreenToWorldPoint(mousePos));
        }
        else if ((lineList.Last().GetPosition(lineList.Last().positionCount - 1) - Camera.main.ScreenToWorldPoint(mousePos)).magnitude> lineWidth)
        {
            crossLine(lineList.Last());
            setLinePosition(Camera.main.ScreenToWorldPoint(mousePos));
        }
        filter.transform.position = centering(lineList.Last());
    }

    boolVector crossLine(LineRenderer line)//交差の確認
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 10.0f;
        boolVector blVect = new boolVector(false, Vector3.zero);
        Vector2[] poins = {
            line.GetPosition(line.positionCount-1),
            Camera.main.ScreenToWorldPoint(mousePos)
        };
        Vector2[] min_max = poins;
        if (min_max[0].x > min_max[1].x)//0を小さく
        {
            var num = min_max[0].x;
            min_max[0].x = min_max[1].x;
            min_max[1].x = num;
        }
        if (min_max[0].y > min_max[1].y)//0を小さく
        {
            var num = min_max[0].y;
            min_max[0].y = min_max[1].y;
            min_max[1].y = num;
        }

        for (int lp=0;lp< line.positionCount - 2; lp++)
        {
            Vector2[] pos =
            {
                line.GetPosition(lp),
                line.GetPosition(lp+1)
            };

            float af = (poins[1].y - poins[0].y) / (poins[1].x - poins[0].x);//1本目変化率
            float at= (pos[1].y - pos[0].y) / (pos[1].x - pos[0].x);//2本目変化率
            float x = (af * poins[0].x - poins[0].y - at * pos[0].x + pos[0].y) / (af - at);
            float y = af * (x - poins[0].x) + poins[0].y;
        }
        return blVect;
    }

    void setLinePosition(Vector3 position)
    {
        if (lineList.Last().positionCount > lineLimit)
        {
            Vector3[] newLinePos = new Vector3[lineList.Last().positionCount - 1];
            for (int lp = 0; lp < lineList.Last().positionCount - 1; lp++)
            {
                newLinePos[lp] = lineList.Last().GetPosition(lp + 1);
            }
            lineList.Last().SetPositions(newLinePos);
            lineList.Last().SetPosition(lineList.Last().positionCount - 1, position);
        }
        else
        {
            lineList.Last().positionCount++;
            lineList.Last().SetPosition(lineList.Last().positionCount - 1, position);
        }
    }

    void checkPoint()
    {
        int fps_par10 = (int)(1 / Time.deltaTime)/10;
        if (lineList.Last().positionCount> fps_par10*2) {
            Vector3[] tp = {
            lineList.Last().GetPosition(lineList.Last().positionCount-1)
            - lineList.Last().GetPosition(lineList.Last().positionCount-fps_par10-1),
            lineList.Last().GetPosition(lineList.Last().positionCount-fps_par10-1)
            - lineList.Last().GetPosition(lineList.Last().positionCount-fps_par10-2)
            };
            //Debug.Log("1:" + Mathf.Atan2(tp[0].x, tp[0].y) * Mathf.Rad2Deg);
            //Debug.Log("2:" + Mathf.Atan2(tp[1].x, tp[1].y) * Mathf.Rad2Deg);
            var ang = Mathf.Atan2(tp[0].x, tp[0].y) * Mathf.Rad2Deg - Mathf.Atan2(tp[1].x, tp[1].y) * Mathf.Rad2Deg;
            Debug.Log(ang);
            if ((int)ang == 90 ||
                (int)ang == -90 ||
                (int)ang == 180 ||
                (int)ang == -180)
            {
                if ((tp[0].x == 0f && tp[0].y == 0f) ||
                (tp[1].x == 0f && tp[1].y == 0f)) return;
                foreach(var tt in tp)
                {
                    Debug.Log(tt);
                }
                Debug.Log(lineList.Last().GetPosition(lineList.Last().positionCount - 1)+"\n"
             +lineList.Last().GetPosition(lineList.Last().positionCount - fps_par10 - 1)+ "\n"
            +lineList.Last().GetPosition(lineList.Last().positionCount - fps_par10 - 1)+ "\n"
            + lineList.Last().GetPosition(lineList.Last().positionCount - fps_par10 - 2));
                //UnityEditor.EditorApplication.isPaused = true;
            }
        }
    }
    void makeLineToObj()
    {
        lineList.Last().loop = true;
        lineList.Last().Simplify(num);
        lineList.Last().transform.position = centering(lineList.Last());
        lineList.Last().SetPositions(centerOfShape(lineList.Last()).ToArray());
        lineList.Last().useWorldSpace = false;
        lineList.Last().gameObject.AddComponent<PolygonCollider2D>();
        lineList.Last().gameObject.AddComponent<linebullet>().destime = 0f;
        var setter= lineList.Last().gameObject.AddComponent<setcol>();
        setter.setSize = 0.50f;
        //lineList.Last().gameObject.AddComponent<MeshFilter>().sharedMesh = makeMesh(lineList.Last());
        //lineList.Last().gameObject.AddComponent<MeshRenderer>();
        TestMesh();
        //checkOnCross(lineList.Last());
    }
    void TestMesh()
    {
        lineList.Last().gameObject.AddComponent<MeshFilter>().sharedMesh = TestmakeMesh();
        lineList.Last().gameObject.AddComponent<MeshRenderer>().material = test;
    }
    Mesh TestmakeMesh()
    {
        List<Vector3> linepos = new List<Vector3>();
        List<int> triangles = new List<int>();
        linepos.Add(centering(lineList.Last()));
        for (int lp = 0; lp < lineList.Last().positionCount; lp++)//点要素の合計から平均化
        {
            linepos.Add(lineList.Last().GetPosition(lp));
            if(lp!=0)
            {
                triangles.Add(0);
                triangles.Add(lp);
                triangles.Add(lp + 1);
            }
            else
            {
                triangles.Add(0);
                triangles.Add(lineList.Last().positionCount);
                triangles.Add(1);
            }
        }
        
        Mesh mesh = new Mesh();
        mesh.vertices = linepos.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    
    Vector2 getFloorAndCeiling(float[] array)//最大xと最小yを返す
    {
        Vector2 num = new Vector2(array[0], array[0]);
        foreach (float Comparison in array)
        {
            num.x = (num.x > Comparison ? num.x : Comparison);
            num.y = (num.y < Comparison ? num.y : Comparison);
        }

        return num;
    }

    float getMassConcentration(LineRenderer line)
    {
        Vector2 points = new Vector2();
        for (int lp = 0; lp < line.positionCount; lp++)
        {
            var point = line.GetPosition(lp);
            points.x += Mathf.Abs(point.x);
            points.y += Mathf.Abs(point.y);
        }
        return points.x * points.y;
    }

    Vector3 centering(LineRenderer line)//中心軸を求める
    {
        Vector3 Shapes = new Vector3();
        for (int lp = 0; lp < line.positionCount; lp++)//点要素の合計
        {
            Shapes += line.GetPosition(lp);
        }
        return Shapes / line.positionCount;
    }

    List<Vector3> centerOfShape(LineRenderer line)//線画の中心を求め,値の変更配列作成
    {
        List<Vector3> center = new List<Vector3>();
        Vector3 cent =centering(line);
        for (int lp = 0; lp < line.positionCount; lp++)//点要素の合計から平均化
        {
            center.Add(line.GetPosition(lp));
            center[lp] -= cent;
        }
        return center;//値のの変更配列
    }

    void checkOnCross(LineRenderer line)//回り方の確認
    {
        List<Vector3> center = new List<Vector3>();
        for (int lp = 0; lp < line.positionCount; lp++)//点要素をリスト化
        {
            center.Add(line.GetPosition(lp));
        }
        float angre = 0;
        List<float> plus = new List<float>();
        List<float> minus = new List<float>();
        for (int lp = 0; lp < line.positionCount-1; lp++)//点要素をリスト化
        {
            if (lp == 0)
            {
                angre = Angr((Vector2)(center[line.positionCount-1] - center[lp]), (Vector2)(center[lp + 1] - center[lp]));

            }
            else 
            {
                angre = Angr((Vector2)(center[lp-1]-center[lp]), (Vector2)(center[lp+1]-center[lp]));
            }

            if (Mathf.Abs(angre) >= 180f&& angre!=0)
            {
                //angre = (angre/ Mathf.Abs(angre)) *360f - angre;
            }

            if (angre > 0)
            {
                plus.Add(angre);
            }else
            {
                minus.Add(angre);
            }
            Debug.Log(angre);
        }
        Debug.Log("plus"+plus.Sum());
        Debug.Log("minus" + minus.Sum());
        Debug.Log("Sum()" + (plus.Sum()+minus.Sum()));
        
    }

    float Angr(Vector2 from,Vector2 to)
    {
        var n= Vector2.Angle(from, to);
        //from = new Vector2(from.x / Mathf.Abs(from.x), from.y / Mathf.Abs(from.y));
        //to = new Vector2(to.x / Mathf.Abs(to.x), to.y / Mathf.Abs(to.y));
        if (n < 0f) n *= -1f;
        if (n > 180f)
        {
            n = 360f - n;
        }
       
        if(((from-to).x==0&& (from - to).y < 0) || ((from - to).y == 0 && (from - to).x < 0))
        {
            //n *= -1f;
        }
        return n;
    }
    void Concentration()
    {
        if (setCol.a != (1f - objRule.gradationTimePer)/2)
        {
            if (objRule.player.GetComponent<player>().onPlay)
            {
                setCol = ConcentCol;
            }
            else
            {
                setCol = HitCol;
            }
            setCol.a = (1f - objRule.gradationTimePer) / 2;
            filter.color = setCol;
        }
    }
}
