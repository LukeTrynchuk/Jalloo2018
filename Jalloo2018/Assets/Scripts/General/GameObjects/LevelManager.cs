using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.Core.Services;
using DogHouse.Jalloo.Services;
using DogHouse.Jalloo.Levels;

public class LevelManager : MonoBehaviour {

    ServiceReference<ILevelGenerator> levelGenerator = new ServiceReference<ILevelGenerator>();

    public PlayfieldData level;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        levelGenerator.AddRegistrationHandle(Register);
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
    // Update is called once per frame
    void Update () {
		
	}
}
