using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.Jalloo.Levels;
using DogHouse.Core.Services;

public interface ILevelManager : IService {
    PlayfieldData GetLevelData();
    void UpdateMap(Entity obj, int previousX,int previousY);
}
