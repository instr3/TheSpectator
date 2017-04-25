using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugText : MonoBehaviour {
    private static DebugText Instance;
    Text text;
	// Use this for initialization
	void Start () {
        Instance = this;
        text = GetComponent<Text>();
    }
	public static void Log(object text)
    {
        Instance.text.text = text.ToString();
    }
    /*private void Update()
    {
        string res = "";
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            res += ("Name = " + hit.collider.name);
            res += ("&Tag = " + hit.collider.tag);
            res += ("&Layer = " + hit.collider.gameObject.layer);
            res += ("&Hit Point = " + hit.point);
            res += ("&Object position = " + hit.collider.gameObject.transform.position);
        }
        else res = "Nothing";
        Log(res);
    }*/
}
