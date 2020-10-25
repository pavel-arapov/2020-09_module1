using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningToEnemy,
        RunningFromEnemy,
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        BeginPunch,
        Punch,
        BeginDying,
        Dead,
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Fist,
    }

    public Weapon weapon;
    public float runSpeed;
    public float distanceFromEnemy;
    public Transform target;
    public TargetIndicator targetIndicator;

    // public AudioClip attackSound;
    // public AudioClip receiveDamageSound;
    // public AudioClip dieSound;
    // public AudioClip footstepSound;

    public string attackSoundName = "StandartHit";
    public string receiveDamageSoundName = "TakeDamageShort";
    public string dieSoundName = "die1";
    public string footstepSoundName = "footstep2";
    
    State state;
    Animator animator;
    Vector3 originalPosition;
    Quaternion originalRotation;
    Health health;
    // private AudioSource _audioSource;
    private AudioPlay _audioPlay;
    
    // access to the states optimised through hashes
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Punch = Animator.StringToHash("Punch");
    private static readonly int Death = Animator.StringToHash("Death");


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        targetIndicator = GetComponentInChildren<TargetIndicator>(true);
        // _audioSource = GetComponent<AudioSource>();
        _audioPlay = GetComponentInChildren<AudioPlay>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public bool IsIdle()
    {
        return state == State.Idle;
    }

    public bool IsDead()
    {
        return state == State.BeginDying || state == State.Dead;
    }

    public void SetState(State newState)
    {
        if (IsDead())
            return;

        state = newState;
    }

    public void DoDamage()
    {
        if (IsDead())
            return;

        health.ApplyDamage(1.0f); // FIXME: захардкожено
        if (health.current <= 0.0f) {
            state = State.BeginDying;
            PlayDieSound();
        }
    }

    [ContextMenu("Attack")]
    public void AttackEnemy()
    {
        if (IsDead())
            return;

        Character targetCharacter = target.GetComponent<Character>();
        if (targetCharacter.IsDead())
            return;

        switch (weapon) {
            case Weapon.Bat:
                state = State.RunningToEnemy;
                break;

            case Weapon.Fist:
                state = State.RunningToEnemy;
                break;

            case Weapon.Pistol:
                state = State.BeginShoot;
                break;
        }
    }

    /**
     * If we have no dynamic weapons we can bind the sound that we need
     */
    public void PlayAttackSound() {
        // _audioSource.PlayOneShot(attackSound);
        _audioPlay.Play(attackSoundName);
    }

    public void PlayReceiveDamageSound() {
        // _audioSource.PlayOneShot(receiveDamageSound);
        _audioPlay.Play(receiveDamageSoundName);
    }

    public void PlayDieSound() {
        // _audioSource.PlayOneShot(dieSound);
        _audioPlay.Play(dieSoundName);
    }

    public void PlayFootstepSound() {
        // _audioSource.PlayOneShot(footstepSound);
        _audioPlay.Play(footstepSoundName);
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat(Speed, 0.0f);
                break;

            case State.RunningToEnemy:
                animator.SetFloat(Speed, runSpeed);
                if (RunTowards(target.position, distanceFromEnemy)) {
                    switch (weapon) {
                        case Weapon.Bat:
                            state = State.BeginAttack;
                            break;

                        case Weapon.Fist:
                            state = State.BeginPunch;
                            break;
                    }
                }
                
                break;

            case State.BeginAttack:
                animator.SetTrigger(MeleeAttack);
                state = State.Attack;
                break;

            case State.Attack:
                break;

            case State.BeginShoot:
                animator.SetTrigger(Shoot);
                state = State.Shoot;
                break;

            case State.Shoot:
                break;

            case State.BeginPunch:
                animator.SetTrigger(Punch);
                state = State.Punch;
                break;

            case State.Punch:
                break;

            case State.RunningFromEnemy:
                animator.SetFloat(Speed, runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;

            case State.BeginDying:
                animator.SetTrigger(Death);
                state = State.Dead;
                break;

            case State.Dead:
                break;
        }
    }
}
