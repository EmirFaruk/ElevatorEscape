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
    private AudioClip breathingSound;

    [SerializeField]
    private AudioClip attackSound;

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

    //   public void PlayBreathingSFX()
    //   {
    //       audioSource.PlayOneShot(breathingSound);
    //   }
    //
    public void PlayAttackSFX()
    {
        audioSource.PlayOneShot(attackSound);
    }


}
