public interface IReplayRecorder
{
    public void PersistReplayDataCapsule(IPlayerAction action);
    public bool SaveReplay();
}