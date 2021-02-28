using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewEventList", menuName ="New List/events")]
public class EventList:ScriptableObject
{
    public List<RuinEvent> events = new List<RuinEvent>();
}
