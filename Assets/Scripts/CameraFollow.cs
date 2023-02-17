using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [Space] 
    [SerializeField] private bool useSmooth;
    [SerializeField] private float smoothTime = .1f;
    private Vector3 _vel;

#if DEBUG
    private void Awake()
    {
        if (target == null) Debug.LogError("Target is null");
    }
#endif

    private void LateUpdate()
    {
        if (!useSmooth)
        {
            transform.position = target.position + offset;
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _vel, smoothTime);
    }
}
