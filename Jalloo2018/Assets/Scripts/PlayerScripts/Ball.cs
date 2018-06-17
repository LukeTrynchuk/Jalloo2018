using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogHouse.Jalloo.Levels;
using System;

public class Ball : Entity
{
    #region Private Variables
    private PlayfieldPosition m_heading;
    #endregion

    #region Main Methods
    public void Launch(Direction direction)
    {
        if (!levelManager.isRegistered()) return;

        SetHeading(direction);
        PlayfieldData data = levelManager.Reference.GetLevelData();

        StartCoroutine(Move(data));
    }
    #endregion

    #region Utility Methods

    private void SetHeading(Direction direction)
    {
        switch (direction)
        {
            case Direction.DOWN:
                m_heading = new PlayfieldPosition(0, -1);
                break;
            case Direction.LEFT:
                m_heading = new PlayfieldPosition(-1, 0);
                break;
            case Direction.RIGHT:
                m_heading = new PlayfieldPosition(1, 0);
                break;
            case Direction.UP:
                m_heading = new PlayfieldPosition(0, 1);
                break;
        }
    }

    private IEnumerator Move(PlayfieldData data)
    {
        bool validMove = true;

        do
        {
            if(!BoundCheck(Position.X + m_heading.X, Position.Y + m_heading.Y))
            {
                if(data.fieldData[Position.X + m_heading.X, Position.Y + m_heading.Y] == EntityType.PLAYER)
                {
                    validMove = false; 
                }

                m_heading.X *= -1;
                m_heading.Y *= -1;
            }

            UpdatePosition(Position.X + m_heading.X, Position.Y + m_heading.Y);
            yield return new WaitForSeconds(0.05f);
            
        } while (validMove);
    }
    #endregion
}
