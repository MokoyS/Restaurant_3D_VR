using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class ObjectManipulator : MonoBehaviour
{
    private bool     _dragging = false;
    private bool     _selected = false;
    private float    _yPlane;
    private Camera   _cam;
    private Renderer _rend;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        _cam  = Camera.main;
        _rend = GetComponentInChildren<Renderer>();
        if (_rend != null) _rend.material.EnableKeyword("_EMISSION");
    }

    void OnMouseDown()
    {
        _selected = true;
        _dragging = true;
        _yPlane   = transform.position.y;
        SetHighlight(true);
    }

    void OnMouseUp()   { _dragging = false; }

    void OnMouseDrag()
    {
        if (!_dragging || _cam == null) return;

        Vector2 mousePos = Mouse.current != null
            ? Mouse.current.position.ReadValue()
            : (Vector2)Input.mousePosition;

        Ray ray = _cam.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, _yPlane, 0f));

        if (groundPlane.Raycast(ray, out float enter))
            transform.position = ray.GetPoint(enter);
    }

    void Update()
    {
        if (!_selected || Keyboard.current == null) return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
            transform.Rotate(Vector3.up, 90f, Space.World);

        if (Keyboard.current.deleteKey.wasPressedThisFrame || Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            SetHighlight(false);
            Destroy(gameObject);
        }
    }

    void OnMouseExit()
    {
        if (!_dragging)
        {
            _selected = false;
            SetHighlight(false);
        }
    }

    private void SetHighlight(bool on)
    {
        if (_rend == null) return;
        _rend.material.SetColor(EmissionColor, on ? Color.white * 0.35f : Color.black);
    }
}
