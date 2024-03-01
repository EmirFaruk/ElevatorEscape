using Cinemachine.Utility;
using RenownedGames.AITree;
using UnityEngine;
using Utilities;

[NodeContent("Slerp", "Tasks/Custom/Slerp")]
public class Slerp : TaskNode
{
    [SerializeField]
    [NonLocal]
    private Key<Transform> target;

    private Transform transform;
    private Transform targetTransform;

    private float counter;

    /// <summary>
    /// Called on behaviour tree is awake.
    /// </summary>
    protected override void OnInitialize()
    {
        base.OnInitialize();
        transform = GetOwner().transform;
    }

    /// <summary>
    /// Called when behaviour tree enter in node.
    /// </summary>
    protected override void OnEntry()
    {
        base.OnEntry();

        if (target == null) return;
 
        targetTransform = target.GetValue();
        counter = 0;
    }

    /// <summary>
    /// Called every tick during node execution.
    /// </summary>
    /// <returns>State.</returns>
    protected override State OnUpdate()
    {
        if (targetTransform == null) return State.Failure;

        Vector3 direction = (targetTransform.position - transform.position).normalized;
        direction.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), counter);
        counter += Time.deltaTime;

        if (counter >= 1) return State.Success;

        return State.Running;




    }

    /// <summary>
    /// Called when behaviour tree exit from node.
    /// </summary>
    protected override void OnExit()
    {
        base.OnExit();
    }
}