using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }

    public AudioClip placeSound;

    private GameObject _prefabToPlace;
    private GameObject _ghost;
    private Camera     _cam;
    private Material   _ghostMat;
    private AudioSource _audio;

    public bool IsPlacing => _prefabToPlace != null;

    void Awake()
    {
        Instance = this;
        _cam = Camera.main;
        _audio = GetComponent<AudioSource>();
        if (_audio == null) _audio = gameObject.AddComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.spatialBlend = 0f;

        _ghostMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        _ghostMat.SetFloat("_Surface", 1f);
        _ghostMat.SetFloat("_Blend", 0f);
        _ghostMat.SetFloat("_SrcBlend", 5f);
        _ghostMat.SetFloat("_DstBlend", 10f);
        _ghostMat.SetFloat("_ZWrite", 0f);
        _ghostMat.renderQueue = 3000;
        _ghostMat.color = new Color(0.4f, 0.8f, 1f, 0.45f);
        _ghostMat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
    }

    public void StartPlacing(GameObject prefab)
    {
        CancelPlacing();
        _prefabToPlace = prefab;
        _ghost = Instantiate(prefab);
        _ghost.name = "Ghost_" + prefab.name;

        foreach (var c in _ghost.GetComponentsInChildren<Collider>()) c.enabled = false;
        foreach (var s in _ghost.GetComponentsInChildren<MonoBehaviour>()) if (s != null) s.enabled = false;
        foreach (var r in _ghost.GetComponentsInChildren<Renderer>())
        {
            var mats = new Material[r.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++) mats[i] = _ghostMat;
            r.materials = mats;
        }
    }

    void Update()
    {
        if (!IsPlacing) return;
        if (_cam == null) _cam = Camera.main;

        Vector2 mousePos = Mouse.current != null
            ? Mouse.current.position.ReadValue()
            : (Vector2)Input.mousePosition;

        Ray ray = _cam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            if (_ghost != null) _ghost.transform.position = hit.point;

        bool rPressed = Input.GetKeyDown(KeyCode.R)
                     || (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame);
        if (_ghost != null && rPressed) _ghost.transform.Rotate(Vector3.up, 90f);

        float scroll = Mouse.current != null
            ? Mouse.current.scroll.ReadValue().y
            : Input.GetAxis("Mouse ScrollWheel") * 100f;
        if (_ghost != null && Mathf.Abs(scroll) > 0.01f)
            _ghost.transform.Rotate(Vector3.up, scroll > 0 ? 15f : -15f);

        bool place = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || Input.GetMouseButtonDown(0);
        if (place) { PlaceObject(hit.point); return; }

        bool cancel = (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
                   || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)
                   || (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame);
        if (cancel) CancelPlacing();
    }

    void PlaceObject(Vector3 position)
    {
        var rot = _ghost != null ? _ghost.transform.rotation : Quaternion.identity;
        var go  = Instantiate(_prefabToPlace, position, rot);
        go.tag  = "Spawned";
        if (go.GetComponent<ObjectManipulator>() == null) go.AddComponent<ObjectManipulator>();
        if (placeSound != null && _audio != null) _audio.PlayOneShot(placeSound);
        CancelPlacing();
    }

    public void CancelPlacing()
    {
        if (_ghost != null) Destroy(_ghost);
        _ghost         = null;
        _prefabToPlace = null;
    }
}
