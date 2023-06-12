using System;
[Serializable]
public abstract class PlayerAction : IPlayerAction, IReplayStateData
{
    public abstract void Execute();
    public enum PlayerActionType
    {
        GridSelection
    }
}
