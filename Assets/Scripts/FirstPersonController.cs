using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed        = 5f;
    public float sprintMultiplier = 1.8f;
    public float mouseSensitivity = 3f;
    public float gravity          = -20f;

    private CharacterController _cc;
    private Transform           _camTransform;
    private float _rotX = 0f;
    private float _velY = 0f;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _camTransform = Camera.main != null ? Camera.main.transform : null;
    }

    void Update()
    {
        bool holdToLook = Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftAlt);
        float mx = Input.GetAxisRaw("Mouse X") * mouseSensitivity * (holdToLook ? 1.5f : 0.5f);
        float my = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * (holdToLook ? 1.5f : 0.5f);

        if (Mathf.Abs(mx) > 0.001f || Mathf.Abs(my) > 0.001f)
        {
            transform.Rotate(Vector3.up, mx, Space.World);
            _rotX = Mathf.Clamp(_rotX - my, -85f, 85f);
            if (_camTransform != null)
                _camTransform.localEulerAngles = new Vector3(_rotX, _camTransform.localEulerAngles.y, 0f);
        }

        float fwd = 0f, side = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    fwd  =  1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  fwd  = -1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  side = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) side =  1f;

        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;
        Vector3 forward = transform.forward; forward.y = 0f; if (forward.sqrMagnitude > 0.001f) forward.Normalize();
        Vector3 right   = transform.right;   right.y   = 0f; if (right.sqrMagnitude > 0.001f)   right.Normalize();

        Vector3 move = (forward * fwd + right * side);
        if (move.sqrMagnitude > 1f) move.Normalize();
        move *= speed;

        _velY = _cc.isGrounded && _velY < 0f ? -2f : _velY + gravity * Time.deltaTime;
        move.y = _velY;
        _cc.Move(move * Time.deltaTime);
    }
}
