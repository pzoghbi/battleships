using System.Threading.Tasks;

public interface IReplayRecorder
{
    public void PersistReplayDataCapsule(IPlayerAction action);
    public Task<bool> SaveReplay();
}