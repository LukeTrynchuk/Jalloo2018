using System;
using DogHouse.Jalloo.Levels;
using UnityEngine;
using System.IO;
using DogHouse.Core.Services;

namespace DogHouse.Jalloo.Services
{
    /// <summary>
    /// The level generator generates the level and
    /// invokes events when a level has been generated.
    /// </summary>
    public class LevelGenerator : MonoBehaviour, ILevelGenerator
    {
        #region Public Variables
        public event Action m_onLevelGenerated;
        public event Action<PlayfieldData> m_onLevelDataGenerated;
        #endregion

        #region Private Variables
        [SerializeField]
        private GameObject[] m_entityPrefabs;

        [SerializeField]
        private string[] m_entityIDs;
        #endregion

        #region Main Methods
        public void GenerateLevel(string filePath)
        {
            if(!File.Exists(filePath))
            {
                Debug.LogError("Invalid file path");
                return;
            }

            PlayfieldData data = GenerateData(filePath);
            CreatePlayField(data);

            m_onLevelGenerated?.Invoke();
            m_onLevelDataGenerated?.Invoke(data);
        }

        void Start() => RegisterService();
        #endregion


        #region Utility Methods
        private void CreatePlayField(PlayfieldData data)
        {
            for (int i = 0; i < data.fieldData.GetLength(0); i++)
            {
                CreateEntityRowInstance(data, i);
            }
        }

        private void CreateEntityRowInstance(PlayfieldData data, int i)
        {
            for (int j = 0; j < data.fieldData.GetLength(0); j++)
            {
                CreateInstance(data, i, j);
            }
        }

        private void CreateInstance(PlayfieldData data, int i, int j)
        {
            for (int currentIndex = 0; currentIndex < m_entityIDs.Length; currentIndex++)
            {
                if(m_entityIDs[currentIndex].Equals(data.fieldData[i,j].ToString()))
                {
                    GameObject entityObject = Instantiate(m_entityPrefabs[currentIndex]);
                    entityObject.transform.position = new Vector3(i, 0f, j);
                }
            }
        }

        private PlayfieldData GenerateData(string filePath)
        {
            string[] lines = GetLineData(filePath);
            return CreateData(lines);
        }

        private PlayfieldData CreateData(string[] lines)
        {
            PlayfieldData data = new PlayfieldData(lines[0].Length, lines.Length);
            for (int i = 0; i < lines.Length; i++)
            {
                GenerateEntity(lines, data, i);
            }
            return data;
        }

        private void GenerateEntity(string[] lines, PlayfieldData data, int index)
        {
            for (int i = 0; i < lines[index].Length; i++)
            {
                switch(lines[index][i])
                {
                    case '_':
                        data.fieldData[index, i] = EntityType.EMPTY;
                        break;

                    case 'E':
                        data.fieldData[index, i] = EntityType.ENEMY;
                        break;

                    case 'P':
                        data.fieldData[index, i] = EntityType.PANEL;
                        break;

                    case 'U':
                        data.fieldData[index, i] = EntityType.PLAYER;
                        break;

                    case 'B':
                        data.fieldData[index, i] = EntityType.BALL;
                        break;

                    case 'X':
                        data.fieldData[index, i] = EntityType.WALL;
                        break;

                    default:
                        Debug.LogError($"Symbol not recognized {lines[index][i]}");
                        break;
                }
            }
        }

        private string[] GetLineData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            lines = FormatLines(lines);
            return lines;
        }

        private string[] FormatLines(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = FormatLine(lines[i]);
            }
            return lines;
        }

        private string FormatLine(string stringToChange) => stringToChange.Trim();

        public void RegisterService()
        {
            ServiceLocator.Register<ILevelGenerator>(this);
        }
        #endregion
    }
}
