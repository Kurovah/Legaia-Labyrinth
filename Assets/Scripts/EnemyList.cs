using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy List", menuName = "New List/EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<EnemyData> enemies = new List<EnemyData>();
}
