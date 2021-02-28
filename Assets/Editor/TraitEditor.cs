using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TraitList))]
public class TraitEditor : Editor
{
    TraitList traitList;
    private void OnEnable()
    {
        traitList = (TraitList)target;
        if(traitList.traits == null)
        {
            traitList.traits = new List<Trait>();
        }
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        foreach (Trait trait in traitList.traits)
        {
            GUILayout.BeginHorizontal();
            trait.visible = EditorGUILayout.Foldout(trait.visible, trait.name);
            if (GUILayout.Button("X"))
            {
                traitList.traits.Remove(trait);
                break;
            }
            GUILayout.EndHorizontal();
            if (trait.visible)
            {
                trait.name = EditorGUILayout.TextField("Name:", trait.name);

                foreach (StatAffector statAffector in trait.affectedStats)
                {
                    GUILayout.BeginHorizontal();
                    statAffector.stat = (StatTypes)EditorGUILayout.EnumPopup(statAffector.stat);
                    statAffector.multiplier = EditorGUILayout.FloatField(statAffector.multiplier);
                    if (GUILayout.Button("X"))
                    {
                        trait.affectedStats.Remove(statAffector);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }

                if(GUILayout.Button("Add affected stat"))
                {
                    trait.affectedStats.Add(new StatAffector());
                }

            }
        }

        if (GUILayout.Button("New Trait"))
        {
            traitList.traits.Add(new Trait());
        }

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(target);
        }

    }

}
