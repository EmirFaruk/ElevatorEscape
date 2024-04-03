using RenownedGames.AITree;
using RenownedGames.AITree.PerceptionSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Zombie : MonoBehaviour
{
    private BehaviourRunner behaviourRunner;
    private Blackboard blackboard;
    private AIPerceptionBlackboard perceptionBlackboard;

    [SerializeField]
    private List<AudioClip> breathingSounds;

    [SerializeField]
    private List<AudioClip> attackSounds;

    [SerializeField]
    private List<AudioClip> screamSounds;

    [SerializeField]
    private List<AudioClip> idleSounds;

    [SerializeField]
    private string wayName;

    private AudioSource audioSource;

    private RigBuilder rigBuilder;

    [SerializeField]
    private Collider hitCollider;

    [SerializeField]
    private float stopDistance;

    private bool canAttack;

    [SerializeField] private Transform player;


    private void Awake()
    {
        behaviourRunner = GetComponent<BehaviourRunner>();
        blackboard = behaviourRunner.GetBlackboard();
        audioSource = GetComponent<AudioSource>();
        perceptionBlackboard = GetComponent<AIPerceptionBlackboard>();

        rigBuilder = GetComponent<RigBuilder>();

        perceptionBlackboard.OnSourceDetect += PerceptionBlackboard_OnSourceDetect;
        perceptionBlackboard.OnSourceLoss += PerceptionBlackboard_OnSourceLoss;

        if (blackboard.TryFindKey("WayName", out StringKey stringKey))
        {
            stringKey.SetValue(wayName);
        }

        if (blackboard.TryFindKey("stopDistance", out FloatKey floatKey))
        {
            floatKey.SetValue(stopDistance);
        }

    }


    private void Update()
    {
        if (blackboard.TryFindKey("distance", out FloatKey floatKey))
        {
            floatKey.SetValue(Vector3.Distance(this.transform.position, player.position));
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            if (blackboard.TryFindKey("canAttack", out BoolKey boolKey))
            {
                canAttack = !canAttack;
                boolKey.SetValue(canAttack);
            }
        }
    }



    private void PerceptionBlackboard_OnSourceLoss(AIPerceptionSource obj)
    {
        rigBuilder.layers[0].rig.weight = 0;
    }

    private void PerceptionBlackboard_OnSourceDetect(AIPerceptionSource obj)
    {
        rigBuilder.layers[0].rig.weight = 1;
    }


    public void OnAttackStart()
    {
        //audioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Count)]);
        PlayAttackSFX();
        hitCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        hitCollider.enabled = false;
    }

    public void OnScreamStart()
    {
        //audioSource.PlayOneShot(screamSounds[Random.Range(0, screamSounds.Count)]);
        AudioManager.OnSFXCall?.Invoke(SoundData.SoundEnum.ZombieBreathing);
    }

    private void PlayAttackSFX()
    {
        if (AudioManager.ZombieAudioSource.clip == null)
        {
            string randomAttackID = UnityEngine.Random
            .Range(((int)SoundData.SoundEnum.ZombieAttack1), ((int)SoundData.SoundEnum.ZombieAttack4) + 1).ToString();
            Enum.TryParse(randomAttackID, out SoundData.SoundEnum value);

            AudioManager.OnAudioSourceSet.Invoke(AudioManager.ZombieAudioSource, value);
        }
    }
}
