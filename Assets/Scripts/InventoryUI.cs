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

    void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
    }

    void Update()
    {
        bool iPressed = Input.GetKeyDown(KeyCode.I)
                     || (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame);
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
