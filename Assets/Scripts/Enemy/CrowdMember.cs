using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.AITree.PerceptionSystem;
using RenownedGames.AITree;

public class CrowdMember : MonoBehaviour
{
    private Crowd crowd;

    private AIPerceptionBlackboard perceptionBlackboard;

    private Blackboard blackboard;
    private BehaviourRunner behaviourRunner;

    private void Awake()
    {
        behaviourRunner = GetComponent<BehaviourRunner>();
        blackboard = behaviourRunner.GetBlackboard();

        perceptionBlackboard = GetComponent<AIPerceptionBlackboard>();
        crowd = transform.GetComponentInParent<Crowd>();
    }

    private void OnEnable()
    {
        perceptionBlackboard.OnSourceDetect += PerceptionBlackboard_OnSourceDetect;
    }

    private void PerceptionBlackboard_OnSourceDetect(AIPerceptionSource obj)
    {
        foreach (CrowdMember member in crowd.GetMembers())
        {        
            if (member != null)
                member.SetTargetLocation(obj.transform);
        }
    }

    public void SetTargetLocation(Transform player)
    {
        if (blackboard.TryFindKey("Player", out TransformKey key))
        {
            key.SetValue(player.transform);
        }
    }
}
