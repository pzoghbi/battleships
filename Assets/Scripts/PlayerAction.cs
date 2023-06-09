public abstract class PlayerAction : IPlayerAction
{
    public abstract void Execute();
    public enum PlayerActionType
    {
        GridSelection
    }
}
