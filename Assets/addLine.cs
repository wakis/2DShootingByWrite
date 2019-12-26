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
    public GameObject testobj;

    // Start is called before the first frame update
    void Start()
    {
        Player = Camera.main.GetComponent<GAMERULE>().Player.GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AddLine_Obj();
        }
        if (Input.GetMouseButton(0))
        {
            AddPositionToLineRend();
        }
        if (Input.GetMouseButtonUp(0))
        {
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
        testobj.transform.position = centering(lineList.Last());
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
            //angle = Angr(poins[0], pos[0]) + Angr(poins[1], pos[0])
            //+ Angr(poins[1], pos[1]) + Angr(poins[0], pos[1]);

            float af = (poins[1].y - poins[0].y) / (poins[1].x - poins[0].x);//1本目変化率
            float at= (pos[1].y - pos[0].y) / (pos[1].x - pos[0].x);//2本目変化率
            float x = (af * poins[0].x - poins[0].y - at * pos[0].x + pos[0].y) / (af - at);
            float y = af * (x - poins[0].x) + poins[0].y;
            if (//min_max[0].x < x && x < min_max[1].x &&
                //min_max[0].y < y && y < min_max[1].y
                min_max[0].y == y * x / min_max[0].x)
            {
               Debug.Log(x+":"+y);
            }
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
        /*List<Vector3> fillMeshPoints = new List<Vector3>();
        for (int lp=0;lp<lineList.Last().positionCount;lp++)
        {
            fillMeshPoints.Add(new Vector2(lineList.Last().GetPosition(lp).x,0f));
            if (lp!= lineList.Last().positionCount-1)
            {
                fillMeshPoints.Add(lineList.Last().GetPosition(lp));
            }
            else
            {
                fillMeshPoints.Add(lineList.Last().GetPosition(0));
            }
        }

        int numTriangles = (lineList.Last().positionCount - 1) * 2;
        int[] triangles = new int[numTriangles * 3];
        int lpInFor = 0;
        for (int lp=0;lp<numTriangles;lp+=2)
        {
            Debug.Log(lpInFor+":"+ numTriangles * 3+":"+ fillMeshPoints.Count);
            // lower left triangle
            triangles[lpInFor++] = 2 * lp;
            triangles[lpInFor++] = 2 * lp + 1;
            triangles[lpInFor++] = 2 * lp + 2;
            // upper right triangle - you might need to experiment what are the correct indices
            triangles[lpInFor++] = 2 * lp + 1;
            triangles[lpInFor++] = 2 * lp + 2;
            triangles[lpInFor++] = 2 * lp + 3;
        }
        Mesh fillMesh = new Mesh();
        fillMesh.vertices = fillMeshPoints.ToArray();
        fillMesh.triangles = triangles;
        return fillMesh;

        */
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
                //triangles.Add(0);
                //triangles.Add(lp+1);
                //triangles.Add(lp);
            }
            else
            {
                triangles.Add(0);
                triangles.Add(lineList.Last().positionCount);
                triangles.Add(1);
                //triangles.Add(0);
                //triangles.Add(1);
                //triangles.Add(lineList.Last().positionCount);
            }
        }
        
        Mesh mesh = new Mesh();
        mesh.vertices = linepos.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    /*Mesh makeMesh(LineRenderer line)//ラインに沿ってメッシュを点在させえる
    {
        int mass = Mathf.FloorToInt(getMassConcentration(line));//点の数
        List<Vector3> points = new List<Vector3>();//点の位置
        List<int> index = new List<int>();//点のインデックス
        List<float> linePosListX = new List<float>();
        List<float> linePosListY = new List<float>();//線の点要素(x,y)
        for (int lp = 0; lp < line.positionCount; lp++)//点要素の合計から平均化
        {
            linePosListX.Add(line.GetPosition(lp).x);
            linePosListY.Add(line.GetPosition(lp).y);
        }

        Vector2[] floor_ceiling =
        {
            getFloorAndCeiling(linePosListX.ToArray()),//[0]Xの最大xと最小y
            getFloorAndCeiling(linePosListY.ToArray())//[1]Yの最大xと最小y
        };
        for (int lp=0;lp< mass;lp++)
        {
            do
            {
                Vector2 randPoint = new Vector2(Random.Range(floor_ceiling[0].y, floor_ceiling[0].x),
                    Random.Range(floor_ceiling[1].y, floor_ceiling[1].x));
            } while ();
        }
    }*/

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
}
