using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class PostFxToggle : MonoBehaviour
{
    Volume volume;

    void Awake() { volume = GetComponent<Volume>(); }

    void Update()
    {
        bool on = FxManager.EffectsOn;
        if (volume.enabled != on) volume.enabled = on;
    }
}