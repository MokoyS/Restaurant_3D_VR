using UnityEngine;

public class CameraPath : MonoBehaviour
{
    [System.Serializable]
    public struct Waypoint
    {
        public Vector3 position;
        public Vector3 lookTarget;
    }

    public Waypoint[] waypoints;
    public float speed = 0.8f;
    public bool loop = true;

    private int currentIndex = 0;
    private float t = 0f;

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
            SetDefaultWaypoints();

        if (waypoints.Length > 0)
            transform.position = waypoints[0].position;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        int nextIndex = (currentIndex + 1) % waypoints.Length;
        t += Time.deltaTime * speed;

        transform.position = Vector3.Lerp(waypoints[currentIndex].position, waypoints[nextIndex].position, t);

        Vector3 target = Vector3.Lerp(waypoints[currentIndex].lookTarget, waypoints[nextIndex].lookTarget, t);
        Vector3 dir = target - transform.position;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3f);

        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            if (!loop && currentIndex == 0) enabled = false;
        }
    }

    void SetDefaultWaypoints()
    {
        waypoints = new Waypoint[]
        {
            new Waypoint { position = new Vector3(-9f, 2.2f,  0f),  lookTarget = new Vector3( 0f, 1f,  0f) },
            new Waypoint { position = new Vector3(-4f, 2.8f, -7f),  lookTarget = new Vector3( 0f, 1f,  0f) },
            new Waypoint { position = new Vector3( 4f, 2.8f, -7f),  lookTarget = new Vector3( 0f, 1f,  0f) },
            new Waypoint { position = new Vector3( 9f, 2.2f,  0f),  lookTarget = new Vector3( 0f, 1f,  0f) },
            new Waypoint { position = new Vector3( 4f, 2.8f,  7f),  lookTarget = new Vector3( 0f, 1f,  0f) },
            new Waypoint { position = new Vector3(-4f, 2.8f,  7f),  lookTarget = new Vector3( 0f, 1f,  0f) },
            new Waypoint { position = new Vector3( 0f, 3.5f,  0f),  lookTarget = new Vector3( 0f, 0f,  0f) },
        };
    }
}
