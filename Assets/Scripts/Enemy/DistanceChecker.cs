using RenownedGames.AITree;
using System;
using UnityEngine;
using RenownedGames.Apex;
using UnityEngine.UIElements;

[NodeContent("DistanceChecker", "Custom/DistanceChecker")]
public class DistanceChecker : ObserverDecorator
{
    public enum Operator
    {
        IsLowerOrEqualTo,
        IsLowerThen,
        IsGreaterOrEqualTo,
        IsGreaterThen

    }


    [Title("Blackboard")]
    [SerializeField]
    [Label("Operator")]
    private Operator compareOperator;

    [SerializeField]
    [NonLocal]
    private TransformKey keyA;

    [SerializeField]
    [NonLocal]
    private TransformKey keyB;

    [SerializeField]
    [NonLocal]
    private FloatKey distanceThreshold;

    public override event Action OnValueChange;


    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnEntry()
    {
        base.OnEntry();
        if (keyA.GetValue() == null || keyB.GetValue() == null) return;
    }


    public override bool CalculateResult()
    {
        if (keyA.GetValue() == null || keyB.GetValue() == null) return false;

        switch (compareOperator)
        {
            case Operator.IsLowerOrEqualTo:
                return Vector3.Distance(keyA.GetValue().position, keyB.GetValue().position) <= distanceThreshold.GetValue();
            case Operator.IsLowerThen:
                return Vector3.Distance(keyA.GetValue().position, keyB.GetValue().position) < distanceThreshold.GetValue();
            case Operator.IsGreaterOrEqualTo:
                return Vector3.Distance(keyA.GetValue().position, keyB.GetValue().position) >= distanceThreshold.GetValue();
            case Operator.IsGreaterThen:
                return Vector3.Distance(keyA.GetValue().position, keyB.GetValue().position) > distanceThreshold.GetValue();
            default:
                return false;
        }
    }
}


