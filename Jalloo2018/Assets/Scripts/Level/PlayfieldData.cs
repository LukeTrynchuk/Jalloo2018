using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.Jalloo.Levels
{
    /// <summary>
    /// The playfield data structure holds 
    /// all the data about a point in time 
    /// of how the playfield looked.
    /// </summary>
    public struct PlayfieldData
    {
        public EntityType[,] fieldData;
        public PlayfieldData(int width, int height)
        {
            fieldData = new EntityType[width,height];
        }
    }
}
