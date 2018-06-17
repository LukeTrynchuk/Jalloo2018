using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;
using DogHouse.Jalloo.Levels;

public class LevelManager : MonoBehaviour,ILevelManager {

    ServiceReference<ILevelGenerator> levelGenerator = new ServiceReference<ILevelGenerator>();

    PlayfieldData level;

    private void OnEnable()
    {
        levelGenerator.AddRegistrationHandle(Register);
        RegisterService();
    }
    private void OnDisable()
    {
        if(levelGenerator.isRegistered())
        {
            levelGenerator.Reference.m_onLevelDataGenerated -= GeneratedLevel;
        }
       
    }
    void Register()
    {
        levelGenerator.Reference.m_onLevelDataGenerated += GeneratedLevel;
    }
    void GeneratedLevel(PlayfieldData data)
    {
        level = data;
    }

    public PlayfieldData GetLevelData()
    {
        return level;
    }

    public void RegisterService()
    {
        ServiceLocator.Register<ILevelManager>(this);
    }

    public void UpdateMap(Entity obj, int previousX, int previousY)
    {
        level.fieldData[previousX, previousY] = EntityType.EMPTY;
        level.fieldData[obj.Position.X, obj.Position.Y] = obj.Type;
    }

    public void SetTileValue(int x, int y, EntityType value)
    {
        level.fieldData[x, y] = value;
    }
}
