using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRInteractor : MonoBehaviour
{
    public float interactRange = 2f;
    public UnityEngine.UI.Text promptLabel;

    private IInteractable _nearest;
    private string _hint;

    private readonly List<UnityEngine.XR.InputDevice> _rightControllers = new List<UnityEngine.XR.InputDevice>();
    private bool _prevA = false;

    Vector3 HeadPosition => Camera.main != null ? Camera.main.transform.position : transform.position;

    void Update()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(
            UnityEngine.XR.InputDeviceCharacteristics.Controller | UnityEngine.XR.InputDeviceCharacteristics.Right,
            _rightControllers);

        bool aHeld = false;
        if (_rightControllers.Count > 0)
            _rightControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out aHeld);
        bool aPressed = aHeld && !_prevA;
        _prevA = aHeld;

        bool ePressed = Input.GetKeyDown(KeyCode.E)
                     || (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                     || aPressed;

        FindNearest();

        if (ePressed && _nearest != null)
            _nearest.Interact();

        UpdatePrompt();
    }

    void FindNearest()
    {
        _nearest = null;
        float bestDist = interactRange;

        foreach (var col in Physics.OverlapSphere(HeadPosition, interactRange))
        {
            var interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;

            float dist = Vector3.Distance(HeadPosition, col.transform.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                _nearest = interactable;
                if (interactable is DoorController)       _hint = "[E / A]  Porte";
                else if (interactable is LightSwitch)     _hint = "[E / A]  Lumières";
                else _hint = $"[E / A]  {(interactable as MonoBehaviour)?.gameObject.name ?? col.gameObject.name}";
            }
        }
    }

    void UpdatePrompt()
    {
        if (promptLabel == null) return;
        var panel = promptLabel.transform.parent?.gameObject ?? promptLabel.gameObject;
        panel.SetActive(_nearest != null);
        if (_nearest != null) promptLabel.text = _hint;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = Camera.main != null ? Camera.main.transform.position : transform.position;
        Gizmos.color = _nearest != null ? Color.green : Color.cyan;
        Gizmos.DrawWireSphere(pos, interactRange);
    }
}
