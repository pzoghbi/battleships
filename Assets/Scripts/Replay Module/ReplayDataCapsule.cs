using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class ReplayDataCapsule : IReplayStateData
{
    /* include data here */
    public uint turn;
    public List<IReplayStateData> boardsState;
    public IReplayStateData actionPlayed;
    public float timestamp;
}