using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.AITree;
using RenownedGames.AITree.PerceptionSystem;
using UnityEngine.Animations.Rigging;
using UnityEngine.AI;

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
        audioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Count)]);
        hitCollider.enabled = true;
    }

    public void OnAttackEnd()
    {
        hitCollider.enabled = false;
    }

    public void OnScreamStart()
    {
        audioSource.PlayOneShot(screamSounds[Random.Range(0, screamSounds.Count)]);
    }

}
