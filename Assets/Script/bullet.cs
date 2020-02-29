using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct BulletrStatus
{
    //public float HP;
    public float At;
    //public float rate;
    public Vector2 size;
    public float speed;
}
public class bullet : MonoBehaviour
{
    public BulletrStatus bStatus;//ステータス
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setBulletStatus(Vector2 size)
    {
        bStatus.At = size.x;
        bStatus.speed = 1f / size.y;
        bStatus.size = size;
    }
}
