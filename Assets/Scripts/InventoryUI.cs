using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject prefabTable;
    public GameObject prefabChaise;
    public GameObject prefabAssiette;
    public GameObject prefabVerre;

    private bool _open = false;

    private readonly List<UnityEngine.XR.InputDevice> _leftControllers = new List<UnityEngine.XR.InputDevice>();
    private bool _prevY = false;

    void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    void Update()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(
            UnityEngine.XR.InputDeviceCharacteristics.Controller | UnityEngine.XR.InputDeviceCharacteristics.Left,
            _leftControllers);

        bool yHeld = false;
        if (_leftControllers.Count > 0)
            _leftControllers[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out yHeld);
        bool yPressed = yHeld && !_prevY;
        _prevY = yHeld;

        bool iPressed = Input.GetKeyDown(KeyCode.I)
                     || (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
                     || yPressed;
        if (iPressed) ToggleInventory();

        bool esc = Input.GetKeyDown(KeyCode.Escape)
                || (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame);
        if (esc && _open) CloseInventory();
    }

    public void ToggleInventory()
    {
        if (_open) CloseInventory();
        else OpenInventory();
    }

    public void OpenInventory()
    {
        _open = true;
        if (inventoryPanel != null) inventoryPanel.SetActive(true);
    }

    public void CloseInventory()
    {
        _open = false;
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    public void SelectTable()      { StartPlacing(prefabTable); }
    public void SelectChaise()     { StartPlacing(prefabChaise); }
    public void SelectAssiette()   { StartPlacing(prefabAssiette); }
    public void SelectVerre()      { StartPlacing(prefabVerre); }

    public void ResetAll()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Spawned"))
            Destroy(go);
        PlacementManager.Instance?.CancelPlacing();
        CloseInventory();
    }

    private void StartPlacing(GameObject prefab)
    {
        if (prefab == null) return;
        CloseInventory();
        PlacementManager.Instance.StartPlacing(prefab);
    }
}
