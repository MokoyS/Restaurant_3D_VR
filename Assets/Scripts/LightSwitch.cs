using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light[] lights;
    public AudioClip switchSound;

    private bool _on = true;
    private AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        if (_audio == null) _audio = gameObject.AddComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.spatialBlend = 1f;
        _audio.maxDistance = 5f;
        _audio.rolloffMode = AudioRolloffMode.Linear;
    }

    void OnMouseDown() => Interact();

    public void Interact()
    {
        _on = !_on;
        if (switchSound != null && _audio != null) _audio.PlayOneShot(switchSound);
        foreach (var l in lights)
            if (l != null) l.enabled = _on;
    }
}
