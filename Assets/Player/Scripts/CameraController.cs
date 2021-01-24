using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private const float zPos = -10;
    private const float interpRate = 10;
    private Transform target;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPosition(Vector2 position)
    {
        Vector3 newPos = new Vector3
        {
            x = position.x,
            y = position.y,
            z = zPos
        };
        transform.position = newPos;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        // Interpolate to target position
        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos.z = zPos;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * interpRate);
        }
    }
}
