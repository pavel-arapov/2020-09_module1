using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void ShootEnd()
    {
        character.SetState(Character.State.Idle);
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void PunchEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void Footstep() {
        character.PlayFootstepSound();
    }

    void DoDamage()
    {
        character.PlayAttackSound();
        character.GetComponent<MuzzleEffectBehaviour>()?.PlayEffect();
        Character targetCharacter = character.target.GetComponent<Character>();
        targetCharacter.GetComponent<HitEffectBehaviour>().PlayEffect();
        targetCharacter.PlayReceiveDamageSound();
        targetCharacter.DoDamage();
    }
}
