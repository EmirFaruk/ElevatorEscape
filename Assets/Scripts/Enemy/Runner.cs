using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.AITree;
using RenownedGames.AITree.PerceptionSystem;

public class Runner : MonoBehaviour
{
    private BehaviourRunner behaviourRunner;
    private Blackboard blackboard;
    private AIPerceptionBlackboard perceptionBlackboard;

    [SerializeField]
    private List<AudioClip> attackSounds;

    [SerializeField]
    private string wayName;

    private AudioSource audioSource;

    private void Awake()
    {
        perceptionBlackboard.OnSourceDetect += PerceptionBlackboard_OnSourceDetect;

        behaviourRunner = GetComponent<BehaviourRunner>();
        blackboard = behaviourRunner.GetBlackboard();

        if (blackboard.TryFindKey("WayName", out StringKey stringKey))
        {
            stringKey.SetValue(wayName);
        }
    }

    private void PerceptionBlackboard_OnSourceDetect(AIPerceptionSource obj)
    {

    }

    public void PlayAttackSFX()
    {
        audioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Count)]);
    }

    public void SetTarget(Transform obj)
    {

    }

}
