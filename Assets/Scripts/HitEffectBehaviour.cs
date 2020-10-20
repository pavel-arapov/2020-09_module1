using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectBehaviour : MonoBehaviour
{
    public ParticleSystem effect;

    public void PlayEffect() {
        effect.Play();
    }
}
