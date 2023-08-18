using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FN_FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private bool follow;
    private Coroutine currentFollow;
    Transform currentObject;
    public enum Type
    {
        Player,
        Object,
    }
    private void Awake()
    {
        Camera.main.orthographicSize = Options.instance.current.viewHeight;
    }
    public Type type;

    public bool FollowState
    {
        get
        {
            return follow;
        }
    }
    public Vector3 offset;
    public Vector2 minCameraBound;
    public Vector2 maxCameraBound;
    public float smoothing;
    [SerializeField]
    private bool useCamBound;

    public bool useSmoothing;

    public int dist;

    void Start()
    {
        smoothing = 16.0f;
    }
    public void Follow()
    {
        follow = true;
    }
    public void Unfollow()
    {
        follow = false;
    }

    public void PlayerFollow(Transform player)
    {
        if (currentFollow != null)
            StopCoroutine(currentFollow);

        type = Type.Player;
        currentObject = player;
        currentFollow = StartCoroutine(FollowPlayer());
    }
    public void ObjectFollow(Transform follow)
    {
        if (currentFollow != null)
            StopCoroutine(currentFollow);

        type = Type.Object;
        currentObject = follow;
        currentFollow = StartCoroutine(FollowObject());
    }

    public void StopFollow()
    {
        if (currentFollow != null)
            StopCoroutine(currentFollow);

        currentObject = null;
        currentFollow = null;
    }
    private IEnumerator FollowPlayer()
    {
        while(currentObject!=null)
        {
            Vector3 desiredPosition;

            desiredPosition = currentObject.position + offset;

            transform.position = desiredPosition;

            yield return new WaitForFixedUpdate();
        }
    }
    private IEnumerator FollowObject()
    {
        while(currentObject != null)
        {
            Vector3 desiredPosition = transform.position;

            if (currentObject.position.x - transform.position.x > dist)
                desiredPosition.x = currentObject.position.x + offset.x - dist;

            if (currentObject.position.x - transform.position.x < -dist)
                desiredPosition.x = currentObject.position.x + offset.x + dist;

            if (currentObject.position.y - transform.position.y > dist)
                desiredPosition.y = currentObject.position.y + offset.y - dist;

            if (currentObject.position.y - transform.position.y < -dist)
                desiredPosition.y = currentObject.position.y + offset.y + dist;

            transform.position = desiredPosition;

            yield return new WaitForEndOfFrame();
        }
    }
}
