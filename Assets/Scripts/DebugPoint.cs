using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugPoint : MonoBehaviour {

    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    public Color Color
    {
        get
        {
            return GetComponent<Image>().color;
        }
        set
        {
            GetComponent<Image>().color = value;
        }
    }
}
