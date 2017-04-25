using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Move2D : MonoBehaviour {
    [HideInInspector]
    public Vector2 WorldPosition;
    public DebugPoint DebugPointPrefab;
    public bool IsDebugMode;
    List<DebugPoint> debugPoints = new List<DebugPoint>();
    Queue<DebugPoint> debugPointCachePool = new Queue<DebugPoint>();
    float relativeCircleSize = 0.015f;
    float dragingMultiplier = 0.001f;
    float keyPressMultiplier = 0.02f;
    float dragingClamp = 0.05f;
    float velocityClamp = 0.1f;
    float linerDrag = 0.05f;
    float wallPushMultiplier = 0.0007f;
    Vector2 dragRelativePosition;
    Vector2 velocity;
    Vector2 mouseDragingPosition;
    Image image;
    Canvas canvas;
    float radius;
    int subdividedRays = 32;
    // Use this for initialization
    void Start () {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.width)* relativeCircleSize;
        canvas = GetComponentInParent<Canvas>();
        radius = GetComponent<RectTransform>().rect.width / 2;
        velocity = Vector2.zero;
        image = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update ()
    {
        
    }
    bool dying = false;
    void StartDie()
    {
        dying = true;
        image.color = Color.red;
    }
    int dieCount = 0;
    private void FixedUpdate()
    {

        /*if (draging)
        {
            velocity += Vector2.ClampMagnitude(dragingMultiplier * (mouseDragingPosition + dragRelativePosition - (Vector2)transform.position), dragingClamp);
        }*/
        if(!dying)
            velocity += keyPressMultiplier * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
        {
            if(image.color.a>0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.01f);
            }
            else
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        //transform.position += (Vector3)velocity;
        ClearDebugPoints();
        if (!RayTest(transform.position))
        {
            ++dieCount;
            if (!dying && dieCount * Time.fixedDeltaTime >= 1f)
                StartDie();
        }
        else dieCount = 0;
        for (int i = 0; i < subdividedRays; ++i)
        {
            float angle = 2 * Mathf.PI / subdividedRays * i;
            Vector2 position = (Vector2)transform.position + radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            if(!RayTest(position))
            {
                velocity -= (position - (Vector2)transform.position) * wallPushMultiplier;
            }
        }
        velocity *= (1 - linerDrag);
        velocity = Vector2.ClampMagnitude(velocity, velocityClamp);
        WorldPosition += velocity;
    }
    
    public void ClearDebugPoints()
    {
        foreach (DebugPoint p in debugPoints)
        {
            p.gameObject.SetActive(false);
            debugPointCachePool.Enqueue(p);
        }
        debugPoints.Clear();
    }
    public bool RayTest(Vector2 source)
    {
        bool result = false;
        RaycastHit hitInfo;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(source), out hitInfo))
        {
            if(hitInfo.collider.gameObject.layer== LayerMask.NameToLayer("Black")
			|| hitInfo.collider.gameObject.layer== LayerMask.NameToLayer("Bg"))
            {
                result=false;
            }
            else
            {
                result=true;
            }
        }
        if (IsDebugMode)
            DrawDebugPointAt(source, result ? Color.green : Color.red);
        return result;
    }
    public void DrawDebugPointAt(Vector2 vec,Color color)
    {
        if (IsDebugMode)
        {
            DebugPoint result;
            if (debugPointCachePool.Count > 0)
            {
                result = debugPointCachePool.Dequeue();
                result.gameObject.SetActive(true);
                result.Position = vec;
            }
            else
            {
                result = Instantiate(DebugPointPrefab, vec, Quaternion.identity) as DebugPoint;
                result.transform.SetParent(canvas.transform);
            }
            result.Color = color;
            debugPoints.Add(result);
        }
    }

}
