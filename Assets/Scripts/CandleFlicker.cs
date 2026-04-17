using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    public Light candleLight;
    public float baseIntensity = 0.5f;
    public float flickerSpeed = 8f;
    public float flickerAmount = 0.25f;

    private float seed;

    void Start()
    {
        seed = Random.Range(0f, 100f);
        if (candleLight == null)
            candleLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (candleLight == null) return;
        float noise = Mathf.PerlinNoise(seed + Time.time * flickerSpeed, seed * 1.5f + Time.time * flickerSpeed * 0.7f);
        candleLight.intensity = baseIntensity + (noise - 0.5f) * 2f * flickerAmount;
        transform.localEulerAngles = new Vector3(
            Mathf.Sin(Time.time * 5f + seed) * 2f,
            0f,
            Mathf.Cos(Time.time * 4f + seed) * 2f
        );
    }
}
