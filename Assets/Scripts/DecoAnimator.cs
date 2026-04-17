using UnityEngine;

public class DecoAnimator : MonoBehaviour
{
    public float amplitude = 0.05f;
    public float bobSpeed = 1.5f;
    public float rotSpeed = 20f;

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * bobSpeed) * amplitude;
        transform.localPosition = _startPos + Vector3.up * offset;
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime, Space.Self);
    }
}
