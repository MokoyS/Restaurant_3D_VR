using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    public Transform doorPivot;
    public float openAngle = 90f;
    public float duration = 0.5f;
    public AudioClip doorSound;

    private bool _open = false;
    private bool _animating = false;
    private AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        if (_audio == null) _audio = gameObject.AddComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.spatialBlend = 1f;
        _audio.maxDistance = 10f;
        _audio.rolloffMode = AudioRolloffMode.Linear;
    }

    void OnMouseDown() => Interact();

    public void Interact()
    {
        if (!_animating && doorPivot != null)
        {
            if (doorSound != null && _audio != null) _audio.PlayOneShot(doorSound);
            StartCoroutine(AnimateDoor());
        }
    }

    IEnumerator AnimateDoor()
    {
        _animating = true;
        float target = _open ? 0f : openAngle;
        float start = doorPivot.localEulerAngles.y;
        if (start > 180f) start -= 360f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float angle = Mathf.Lerp(start, target, t);
            doorPivot.localEulerAngles = new Vector3(0f, angle, 0f);
            yield return null;
        }

        doorPivot.localEulerAngles = new Vector3(0f, target, 0f);
        _open = !_open;
        _animating = false;
    }
}
