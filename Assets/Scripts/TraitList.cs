using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New trait list", menuName = "New List/traits")]
[System.Serializable]
public class TraitList : ScriptableObject
{
    public List<Trait> traits = new List<Trait>();
}
