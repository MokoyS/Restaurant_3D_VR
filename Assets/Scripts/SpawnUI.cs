using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnUI : MonoBehaviour
{
    public ObjectSpawner spawner;

    void Start()
    {
        if (spawner == null) spawner = ObjectSpawner.Instance;
    }

    void Update()
    {
        bool k1 = Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)
                || (Keyboard.current != null && (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame));
        bool k2 = Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)
                || (Keyboard.current != null && (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame));
        bool k3 = Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)
                || (Keyboard.current != null && (Keyboard.current.digit3Key.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame));
        bool k4 = Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)
                || (Keyboard.current != null && (Keyboard.current.digit4Key.wasPressedThisFrame || Keyboard.current.numpad4Key.wasPressedThisFrame));

        if (k1) SpawnTable();
        if (k2) SpawnChair();
        if (k3) SpawnDecoration();
        if (k4) ResetScene();
    }

    public void SpawnTable()      { spawner?.Spawn(0); }
    public void SpawnChair()      { spawner?.Spawn(1); }
    public void SpawnDecoration() { spawner?.Spawn(2); }
    public void ResetScene()      { spawner?.ResetAll(); }
}
