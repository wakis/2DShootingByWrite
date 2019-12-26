using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAMERULE : MonoBehaviour
{
    public GameObject Player;
    public GameObject Boss;
    public float timeOnGame;
    public float sceneProgress;

    public Vector2[] ScreenSize = new Vector2[2];
    public float scrollSpeed;
    private void Awake()
    {
        float camZ = Camera.main.transform.position.z;
        ScreenSize[0] = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -camZ));
        ScreenSize[1] = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -camZ));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
