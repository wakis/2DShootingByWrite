using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class selectScene : MonoBehaviour
{
    [SerializeField]
    string[] NormalScene;
    [SerializeField]
    string[] HardScene;
    [SerializeField]
    string[] ExScene;
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform[] point = new Transform[4];

    public Material lineMaterial;//線のMaterial
    public Color stlineColor; //線の色
    public Color edlineColor;
    public float lineWidth = 0.1f;//線の太さ

    static public Vector2 playerPos;

    [SerializeField]
    Material meshMat;

    int pointNum;
    int nextPoint;

    LineRenderer line = new LineRenderer();
    GameObject lineobj;

    [SerializeField]
    AudioClip selectSE;
    [SerializeField]
    AudioClip doneSE;

    AudioSource Audio;

    private void Awake()
    {
        pointNum = nextPoint= 0;
        player.position = point[pointNum].position;
        line = new LineRenderer();
        Audio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.position;
        int n = -(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")>0?
            Mathf.CeilToInt(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")): 
            Mathf.FloorToInt(Input.GetAxis("Horizontal") + Input.GetAxis("Vertical"))) ;
        if ((n > 0|| n < 0)&&Input.anyKeyDown)
        {
            Audio.PlayOneShot(selectSE);
            selectPos(n);
        }

        if (Input.GetButtonDown("Linewrite"))
        {
            AddLine_Obj();
        }
        if (Input.GetButton("Linewrite"))
        {
            AddPositionToLineRend();
        }
        if (Input.GetButtonUp("Linewrite"))
        {
            if (line.positionCount < 3)//要素数不足
            {
                Destroy(line.gameObject);
            }
            else
            {
                makeLineToObj();
                if (lineobj != null && lineobj.GetComponent<bullet>())
                {
                    for (int lp = 1; lp < 4; lp++)
                    {
                        if (lineobj.transform.position.x - lineobj.GetComponent<bullet>().bStatus.size.x / 2 < (point[lp].position - point[lp].localPosition).x &&
                            lineobj.transform.position.x + lineobj.GetComponent<bullet>().bStatus.size.x / 2 > (point[lp].position - point[lp].localPosition).x &&
                            lineobj.transform.position.y - lineobj.GetComponent<bullet>().bStatus.size.y / 2 < (point[lp].position - point[lp].localPosition).y &&
                            lineobj.transform.position.y + lineobj.GetComponent<bullet>().bStatus.size.y / 2 > (point[lp].position - point[lp].localPosition).y)
                        {
                            pointNum = nextPoint = lp;
                            player.position = point[pointNum].position;
                        }
                    }
                }
            }
        }
        player.position = point[pointNum].position;
        sceneSelect();
    }
    void sceneSelect()
    {
        Debug.Log(pointNum);
        if (Input.GetButtonDown("Shot"))
        {
            Audio.PlayOneShot(doneSE);
            int n = 0;
            switch (pointNum)
            {
                case 1:
                    n = (int)Random.Range(0, NormalScene.Length);
                    SceneManager.LoadSceneAsync(NormalScene[n]);
                    break;
                case 2:
                    n = (int)Random.Range(0, HardScene.Length);
                    SceneManager.LoadSceneAsync(HardScene[n]);
                    break;
                case 3:
                    n = (int)Random.Range(0, ExScene.Length);
                    SceneManager.LoadSceneAsync(ExScene[n]);
                    break;
            }
        }
    }
    void selectPos(int n)
    {
        player.position = point[pointNum].position;
        nextPoint = pointNum + n;
        if (nextPoint < 1)
        {
            nextPoint = 2;
        }
        else if (nextPoint > 2)
        {
            nextPoint = 1;
        }
        pointNum = nextPoint;
    }
    bool AddLine_Obj()
    {
        if (lineobj != null)
        {
            Destroy(lineobj);
        }
        //線オブジェクト生成、をリストに追加
        GameObject bfLine = new GameObject();
        bfLine.transform.parent = Camera.main.transform;
        line = bfLine.AddComponent<LineRenderer>();
        Debug.Log(line.name);

        //線オブジェクトの初期化
        line.positionCount = 0;
        line.material = lineMaterial;
        line.material.color = stlineColor;
        line.startColor = stlineColor;
        line.endColor = edlineColor;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        return true;
    }

    void AddPositionToLineRend()//クリックドラック中
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 10.0f;
        if (line.positionCount == 0)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, Camera.main.ScreenToWorldPoint(mousePos));
        }
        else if ((line.GetPosition(line.positionCount - 1) - Camera.main.ScreenToWorldPoint(mousePos)).magnitude > lineWidth)
        {
            setLinePosition(Camera.main.ScreenToWorldPoint(mousePos));
        }
    }
    void setLinePosition(Vector3 position)
    {
        if (line.positionCount > 100)
        {
            Vector3[] newLinePos = new Vector3[line.positionCount - 1];
            for (int lp = 0; lp < line.positionCount - 1; lp++)
            {
                newLinePos[lp] = line.GetPosition(lp + 1);
            }
            line.SetPositions(newLinePos);
            line.SetPosition(line.positionCount - 1, position);
        }
        else
        {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, position);
        }
    }
    void makeLineToObj()
    {
        line.loop = true;
        line.Simplify(0);
        line.transform.position = centering(line);
        line.SetPositions(centerOfShape(line).ToArray());
        line.useWorldSpace = false;
        line.gameObject.AddComponent<PolygonCollider2D>();
        var setter = line.gameObject.AddComponent<setcol>();
        setter.setSize = 0f;
        TestMesh();
        lineobj = line.gameObject;
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
        Vector3 cent = centering(line);
        for (int lp = 0; lp < line.positionCount; lp++)//点要素の合計から平均化
        {
            center.Add(line.GetPosition(lp));
            center[lp] -= cent;
        }
        return center;//値のの変更配列
    }
    void TestMesh()
    {
        line.gameObject.AddComponent<MeshFilter>().sharedMesh = TestmakeMesh();
        line.gameObject.AddComponent<MeshRenderer>().material = meshMat;
    }
    Mesh TestmakeMesh()
    {
        List<Vector3> linepos = new List<Vector3>();
        List<int> triangles = new List<int>();
        linepos.Add(centering(line));
        for (int lp = 0; lp < line.positionCount; lp++)//点要素の合計から平均化
        {
            linepos.Add(line.GetPosition(lp));
            if (lp != 0)
            {
                triangles.Add(0);
                triangles.Add(lp);
                triangles.Add(lp + 1);
            }
            else
            {
                triangles.Add(0);
                triangles.Add(line.positionCount);
                triangles.Add(1);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = linepos.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;
    }
}
