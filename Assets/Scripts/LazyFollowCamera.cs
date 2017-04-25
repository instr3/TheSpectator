using UnityEngine;
using System.Collections;

public class LazyFollowCamera : MonoBehaviour {
    public Move2D follower;
    Vector2 currentPosition;
    float followSpeed=0.05f;
    float transformY;
    Camera camera;
	// Use this for initialization
	void Start () {
        currentPosition = follower.WorldPosition;
        transformY = transform.position.y;
        camera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        currentPosition = Vector2.Lerp(currentPosition, follower.WorldPosition, followSpeed);
        camera.transform.position = new Vector3(currentPosition.x, transformY, currentPosition.y);
        follower.transform.position = camera.WorldToScreenPoint(new Vector3(follower.WorldPosition.x, 0, follower.WorldPosition.y));
	}
}
