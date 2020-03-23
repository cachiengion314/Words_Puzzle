using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Create Data", order = 0)]
public class GameData : ScriptableObject
{
    public List<LevelsData> gameLevels;
}
