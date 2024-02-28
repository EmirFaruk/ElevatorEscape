using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenownedGames.AITree;
using RenownedGames.AITree.PerceptionSystem;

public class Zombie : MonoBehaviour
{
    private BehaviourRunner behaviourRunner;
    private Blackboard blackboard;

    private AIPerceptionBlackboard perceptionBlackboard;


    private Transform player;

    private void Awake()
    {
        behaviourRunner = GetComponent<BehaviourRunner>();
        blackboard = behaviourRunner.GetBlackboard();    
    }  

    private void PerceptionBlackboard_OnSourceDetect(AIPerceptionSource obj)
    {
        // eger crowd member isen 
        // diger crowd uyelerine player transform bilgisi gönder.

        if (blackboard.TryFindKey("Player", out TransformKey key))
        {
            key.SetValue(obj.transform);
        }
    }
     

    public Blackboard GetBlackboard() { return blackboard; }

}
