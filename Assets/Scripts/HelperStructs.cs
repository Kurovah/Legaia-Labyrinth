using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatTypes
{
    maxHp,
    spd,
    atk
}

public enum TraitType
{
    job,
    race
}

public enum Races
{
    Human,
    Kobold,
    Fae
}

public enum Jobs
{
    warrior,
    healer,
    wizard,
    rouge
}

public enum AttackMethod
{
    attackSingle,
    attackAll,
    Heal
}

[System.Serializable]
public class Trait
{
    public bool visible; 
    public string name;

    public List<StatAffector> affectedStats;

    public Trait()
    {
        name = "New Trait";
        affectedStats = new List<StatAffector>();
    }
}

[System.Serializable]
public class StatAffector
{
    public float multiplier;
    public StatTypes stat;

    public StatAffector()
    {
        multiplier = 0;
        stat = StatTypes.maxHp;
    }
}

[System.Serializable]
public class Stats
{
    public int maxHp,spd,atk,level;

    public Stats()
    {
        level = 1;
        maxHp = 10;
        spd=atk= 1;
    }
}



[System.Serializable]
public class MemberData
{
    public string memberName;
    public Stats stats = new Stats();
    public int jobTraitIndex,raceTraitIndex;
}

[System.Serializable]
public class EnemyData
{
    public string name;
    public Stats enemyStats;
}

[System.Serializable]
public class PartyData
{
    public int Essence = 0;
    public List<MemberData> members = new List<MemberData>();
}

public enum RuinEventType
{
    AffectAll,
    CheckClass,
    Battle
}

public enum RuinEventEffect
{
    DamageAll,
    HealAll,
    Revive,
    GiveEssence
}

[System.Serializable]
public class RuinEvent
{
    public bool visibleInEditor;
    public RuinEventType ruinEventType;
    public RuinEventEffect eventEffect;
    public Jobs targetClass;
    public string name, startText, passText, failText;
}

public class SaveSystem
{
    public static void SavePartyData(PartyData pd)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/party.data";
        FileStream fs = new FileStream(path, FileMode.Create);

        bf.Serialize(fs, pd);
        fs.Close();
    }
    public static PartyData GetPartyData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/party.data";
        FileStream fs = new FileStream(path, FileMode.Open);
        PartyData pd =  bf.Deserialize(fs) as PartyData;
        fs.Close();
        return pd;
    }
}
