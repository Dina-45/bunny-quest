using System.Collections;
using UnityEngine;

// Белая вспышка при ударе через Shader Graph (_FlashAmount).
// MaterialPropertyBlock: значение меняется на этом объекте, без нового материала на каждый удар.
[RequireComponent(typeof(SpriteRenderer))]
public class HitFlash : MonoBehaviour
{
    static readonly int FlashId = Shader.PropertyToID("_FlashAmount");

    SpriteRenderer sr;
    MaterialPropertyBlock mpb;
    Coroutine running;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void Flash()
    {
        if (!FxManager.EffectsOn) return;
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(FlashRoutine(FxManager.FlashTime));
    }

    IEnumerator FlashRoutine(float time)
    {
        for (float t = 0f; t < time; t += Time.unscaledDeltaTime)   // работает и во время hit stop
        {
            Set(1f - t / time);        // сразу белый, потом плавно обратно
            yield return null;
        }
        Set(0f);
        running = null;
    }

    void Set(float amount)
    {
        sr.GetPropertyBlock(mpb);      // сохраняем то, что уже передал SpriteRenderer (картинку)
        mpb.SetFloat(FlashId, amount);
        sr.SetPropertyBlock(mpb);
    }

    void OnDisable()
    {
        if (sr != null && mpb != null) Set(0f);   // не оставлять объект белым
    }
}