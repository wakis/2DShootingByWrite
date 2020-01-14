using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desAnimeObject : MonoBehaviour
{
    [SerializeField]
    GameObject connectEffect;
    [System.NonSerialized]
    public bool connect = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!connectEffect)
        {
            connect = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var transRot = transform.eulerAngles;
        transRot.z += (int)(Time.deltaTime * Random.Range(0, 90));
        transform.eulerAngles = transRot;
    }

    public void destroy_AnimationObject()
    {
        Debug.Log("Dess");
        if (connect)
        {
            Instantiate(connectEffect, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-2f, 2f), 0f)
                , Quaternion.Euler(transform.eulerAngles)).GetComponent<desAnimeObject>().connect = connect;
        }
        Destroy(gameObject);
    }
}
