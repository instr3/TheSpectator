using UnityEngine;
using System.Collections;

public class DragAndMove : MonoBehaviour {
    public enum SpinAxisEnum
    {
        Top=1,
        Forward=2,
        None=0
    }
    public SpinAxisEnum SpinAxis;
    private float smoothDamp = 0.3f;
    private float rotateMultiplierForward = 0.2f;
    private float rotateMultiplierTop = 0.4f;
    private bool isRotating;
    private new Rigidbody rigidbody;
    private float saveDrag, saveAngleDrag;

    void Start()
    {
        Debug.Log(Vector3.Angle(Vector3.up, Vector3.forward));
        Debug.Log(Vector3.Angle(Vector3.forward, Vector3.up));
        rigidbody = GetComponent<Rigidbody>();
        saveDrag = rigidbody.drag;
        saveAngleDrag = rigidbody.angularDrag;
    }
    private Vector3 mouseReference;
    float smoothYpos;
    float smoothXpos;
    void FixedUpdate()
    {
        if (isRotating)
        {
            Vector3 mouseOffset = (Input.mousePosition - mouseReference);
            float ypos = mouseOffset.x;
            float xpos = mouseOffset.y;
            //DebugText.Log(new Vector2(xpos, ypos));
            smoothYpos = Mathf.Lerp(smoothYpos, ypos, smoothDamp);
            smoothXpos = Mathf.Lerp(smoothXpos, xpos, smoothDamp);
            rigidbody.angularVelocity = Vector3.zero;
            switch (SpinAxis)
            {
                case SpinAxisEnum.None:
                    break;
                case SpinAxisEnum.Forward:
                    float deltaPosition = transform.forward.x * smoothXpos - transform.forward.z * smoothYpos;
                    rigidbody.AddTorque(transform.forward * deltaPosition * rotateMultiplierForward, ForceMode.VelocityChange);
                    // transform.Rotate(Camera.main.transform.up, -(mouseOffset.x) * sensitivity, Space.World);
                    break;
                case SpinAxisEnum.Top:
                    Vector3 spinnerCenter = Camera.main.WorldToScreenPoint(transform.position);
                    float mouseAngle = Vector3.Angle(
                        Input.mousePosition - spinnerCenter,
                        mouseReference - spinnerCenter);
                    if (Vector3.Cross(Input.mousePosition - spinnerCenter, mouseReference - spinnerCenter).z < 0)
                        mouseAngle = -mouseAngle;
                    rigidbody.AddTorque(transform.forward * mouseAngle * rotateMultiplierTop, ForceMode.VelocityChange);
                    break;
            }
            mouseReference = Input.mousePosition;
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Down");
        // rotating flag
        isRotating = true;
        // store mouse
        mouseReference = Input.mousePosition;
        rigidbody.drag = 0;
        rigidbody.angularDrag = 0;
    }

    void OnMouseUp()
    {
        Debug.Log("Up");
        // rotating flag
        isRotating = false;
        rigidbody.drag = saveDrag;
        rigidbody.angularDrag = saveAngleDrag;
    }
}
