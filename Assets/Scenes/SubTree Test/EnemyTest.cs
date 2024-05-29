using RenownedGames.AITree;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField]
    BehaviourRunner behaviourRunner;

    [SerializeField]
    private Animator animator;

    Blackboard blackboard;

    [SerializeField]
    private Transform player;

    private void Awake()
    {
        blackboard = behaviourRunner.GetBlackboard();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //  if(blackboard.tr)
            if (blackboard.TryFindKey("Player", out TransformKey transFormKey))
            {
                transFormKey.SetValue(player);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (blackboard.TryFindKey("isInAttackRange", out BoolKey boolKey))
            {
                boolKey.SetValue(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (blackboard.TryFindKey("isInAttackRange", out BoolKey boolKey))
            {
                boolKey.SetValue(false);
            }
        }
    }
}
