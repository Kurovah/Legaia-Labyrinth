using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventList))]
public class EventListEditor : Editor
{
    EventList eventList;
    private void OnEnable()
    {
        eventList = (EventList)target;
        if(eventList.events == null) { eventList.events = new List<RuinEvent>(); }
    }

    public override void OnInspectorGUI()
    {

        foreach(RuinEvent ruinEvent in eventList.events)
        {
            GUILayout.BeginHorizontal();
            ruinEvent.visibleInEditor = EditorGUILayout.Foldout(ruinEvent.visibleInEditor, ruinEvent.name);
            if (GUILayout.Button("X"))
            {
                eventList.events.Remove(ruinEvent);
                break;
            }
            GUILayout.EndHorizontal();
            
            if (ruinEvent.visibleInEditor)
            {
                ruinEvent.name = EditorGUILayout.TextField("Event Name", ruinEvent.name);
                ruinEvent.ruinEventType = (RuinEventType)EditorGUILayout.EnumPopup(ruinEvent.ruinEventType);
                switch (ruinEvent.ruinEventType)
                {
                    case RuinEventType.Battle:

                        break;
                    case RuinEventType.AffectAll:
                        ruinEvent.startText = EditorGUILayout.TextField("Start Text", ruinEvent.startText);
                        ruinEvent.passText = EditorGUILayout.TextField("Pass Text", ruinEvent.passText);
                        ruinEvent.eventEffect = (RuinEventEffect)EditorGUILayout.EnumPopup(ruinEvent.eventEffect);
                        break;
                    case RuinEventType.CheckClass:
                        ruinEvent.startText = EditorGUILayout.TextField("Start Text", ruinEvent.startText);
                        ruinEvent.passText = EditorGUILayout.TextField("Pass Text", ruinEvent.passText);
                        ruinEvent.failText = EditorGUILayout.TextField("Fail Text", ruinEvent.failText);
                        ruinEvent.targetClass = (Jobs)EditorGUILayout.EnumPopup(ruinEvent.targetClass);
                        break;
                }
            }
        }

        if (GUILayout.Button("New Event Type"))
        {
            eventList.events.Add(new RuinEvent());
        }
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(target);
        }
    }
}
