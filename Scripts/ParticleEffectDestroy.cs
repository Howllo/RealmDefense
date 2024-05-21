using System;
using System.Collections;
using UnityEngine;

public class ParticleEffectDestroy : MonoBehaviour
{
    public float endOfParticleAnimation;
    public ParticleSystem particleSystem;

    private void Start()
    {
        StartCoroutine(DestroyParticle());
    }

    private IEnumerator DestroyParticle()
    {
        yield return new WaitForSeconds(endOfParticleAnimation);
        Destroy(gameObject);
    }
}
