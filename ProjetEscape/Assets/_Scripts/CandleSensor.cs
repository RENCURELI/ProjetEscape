using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CandleSensor : MonoBehaviour
{
    public bool triggered = false;
    public float radius = 5f;
    public float litFactor = 1;

    public new Renderer renderer;
    private MaterialPropertyBlock mpb;

    public const float fadeTime = 1;

    public void SetLitFactor(float value)
    {
        if (mpb == null) mpb = new MaterialPropertyBlock();
        litFactor = value;
        mpb.SetFloat("_FadeFactor", Mathf.Clamp01( 1- litFactor));
        renderer.SetPropertyBlock(mpb);
    }

    private void Awake()
    {
        gameObject.AddComponent<CandleBehaviour>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
