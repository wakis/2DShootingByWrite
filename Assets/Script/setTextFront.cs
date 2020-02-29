using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTextFront : MonoBehaviour
{
    [SerializeField, ContextMenuItem("SetLayer", "SetLayer")]
    string layerName;
    [SerializeField,ContextMenuItem("SetLayer", "SetLayer")]
    int sortingNum;
    void SetLayer()
    {
        GetComponent<Renderer>().sortingLayerName = layerName;
        GetComponent<Renderer>().sortingOrder = sortingNum;
    }

}
