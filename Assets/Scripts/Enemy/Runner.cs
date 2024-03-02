using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.AITree;
using RenownedGames.AITree.PerceptionSystem;
using UnityEngine.Animations.Rigging;

public class Runner : MonoBehaviour
{
    private BehaviourRunner behaviourRunner;
    private Blackboard blackboard;
    private AIPerceptionBlackboard perceptionBlackboard;

    [SerializeField]
    private List<AudioClip> attackSounds;

    [SerializeField]
    private string wayName;

    private RigBuilder rigBuilder;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        perceptionBlackboard = GetComponent<AIPerceptionBlackboard>();
        rigBuilder = GetComponent<RigBuilder>();


        behaviourRunner = GetComponent<BehaviourRunner>();
        blackboard = behaviourRunner.GetBlackboard();

        perceptionBlackboard.OnSourceDetect += PerceptionBlackboard_OnSourceDetect;
        perceptionBlackboard.OnSourceLoss += PerceptionBlackboard_OnSourceLoss;

        if (blackboard.TryFindKey("WayName", out StringKey stringKey))
        {
            stringKey.SetValue(wayName);
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

    public void PlayAttackSFX()
    {
        audioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Count)]);
    }

    public void SetTarget(Transform obj)
    {

    }

}
