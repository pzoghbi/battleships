﻿using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReplayData
{
    public Task<bool> SaveToFile();
    public void UpdateState(List<IReplayStateData> stateToUpdate, IReplayStateData stateData);
}