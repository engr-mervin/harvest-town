using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector2 minCameraBound;
    public Vector2 maxCameraBound;
    Vector3 desiredPosition;
    Vector3 smoothedPosition;
    public float smoothing;
    Vector3 velocity = Vector3.zero;

    void Start()
    {
        smoothing = Uprefs.cameraSmoothing;
    }
    void FixedUpdate()
    {
        if (player == null) return;
        

        desiredPosition.x = Mathf.Clamp(player.position.x+offset.x, minCameraBound.x, maxCameraBound.x);
        desiredPosition.y = Mathf.Clamp(player.position.y+offset.y, minCameraBound.y, maxCameraBound.y);
        desiredPosition.z = player.position.z + offset.z;

        smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothing*Time.deltaTime);

        transform.position = smoothedPosition;


    }
}
