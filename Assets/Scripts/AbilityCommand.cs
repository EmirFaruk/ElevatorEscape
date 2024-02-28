﻿public class AbilityCommand : ICommand
{
    private readonly AbilityData data;
    public float duration => data.duration;

    public AbilityCommand(AbilityData data)
    {
        this.data = data;
    }

    public void Execute()
    {
        /* EventBus<PlayerAnimationEvent>.Raise(new PlayerAnimationEvent {
             animationHash = data.animationHash
         });*/
    }
}

public interface ICommand
{
    void Execute();
}

public struct PlayerAnimationEvent //: IEvent
{
    public int animationHash;
}