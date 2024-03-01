using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.AITree;
using RenownedGames.AITree.PerceptionSystem;

public class Zombie : MonoBehaviour
{
    private BehaviourRunner behaviourRunner;
    private Blackboard blackboard;

    [SerializeField]
    private List<AudioClip> breathingSounds;

    [SerializeField]
    private List<AudioClip> attackSounds;

    [SerializeField]
    private string wayName;

    private AudioSource audioSource;

    private void Awake()
    {
        behaviourRunner = GetComponent<BehaviourRunner>();
        blackboard = behaviourRunner.GetBlackboard();
        audioSource = GetComponent<AudioSource>();

        if (blackboard.TryFindKey("WayName", out StringKey stringKey))
        {
            stringKey.SetValue(wayName);
        }
    }

    public void PlayBreathingSFX()
    {
      //  audioSource.PlayOneShot(breathingSounds[0]);    
    }

    public void PlayAttackSFX()
    {
        audioSource.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Count)]);
    }


}
