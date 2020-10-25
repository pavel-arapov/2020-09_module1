using UnityEngine;

public class MuzzleEffectBehaviour : MonoBehaviour
{
    public ParticleSystem effect;

    public void PlayEffect() {
        effect.Play();
    }    
}