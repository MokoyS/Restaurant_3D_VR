using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;

    public GameObject[] spawnables;
    public Vector3 spawnCenter = new Vector3(0f, 0f, 0f);

    private readonly List<GameObject> _spawned = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void Spawn(int index)
    {
        if (spawnables == null || index < 0 || index >= spawnables.Length) return;
        if (spawnables[index] == null) return;

        GameObject go = Instantiate(spawnables[index], spawnCenter, Quaternion.identity);
        go.tag = "Spawned";

        if (go.GetComponent<ObjectManipulator>() == null)
            go.AddComponent<ObjectManipulator>();

        _spawned.Add(go);
    }

    public void ResetAll()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Spawned"))
            Destroy(go);
        _spawned.Clear();
    }
}
