using UnityEngine;

public class FallDetector : MonoBehaviour
{
    public Vector3 respawnPosition = new Vector3(0f, 0.1f, -4f);

    void OnTriggerEnter(Collider other)
    {
        var root = other.transform.root;
        if (!root.name.Contains("XR Origin") && !root.CompareTag("Player")) return;

        var cc = root.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            root.position = respawnPosition;
            cc.enabled = true;
        }
        else
        {
            root.position = respawnPosition;
        }
    }
}
