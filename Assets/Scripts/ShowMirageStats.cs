using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMirageStats : MonoBehaviour
{
    public HomeBaseBehaviour home;
    public MemberData mb;
    public Text jobText, raceText, LevelupButtonText, namePlateText, hpText, spdtxt, atkStxt;
    public List<Sprite> bodySprites,headSprites;
    public Image headSprite, bodySprite;
    public GameObject levelUpFx;
    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void IncrementJob()
    {
        if (mb.jobTraitIndex < 3)
        {
            mb.jobTraitIndex++;
        }
        else
        {
            mb.jobTraitIndex = 0;
        }
    }

    public void IncrementRace()
    {
        if(mb.raceTraitIndex < 2)
        {
            mb.raceTraitIndex++;
        } else
        {
            mb.raceTraitIndex = 0;
        }
    }

    public void UpdateText()
    {
        raceText.text = "Race:"+((Races)mb.raceTraitIndex).ToString();
        jobText.text = "Job:"+((Jobs)mb.jobTraitIndex).ToString();
        LevelupButtonText.text = "Level Up Cost:" + (mb.stats.level * 100).ToString();
        namePlateText.text = mb.memberName + ":Lv " + mb.stats.level;
        headSprite.sprite = headSprites[mb.raceTraitIndex];
        bodySprite.sprite = bodySprites[mb.jobTraitIndex];
        hpText.text = "MaxHP:" + GetTraitMultiplier(StatTypes.maxHp, mb.stats.maxHp);
        atkStxt.text = "Atk:" + GetTraitMultiplier(StatTypes.atk, mb.stats.atk);
        spdtxt.text = "Spd:" + GetTraitMultiplier(StatTypes.spd, mb.stats.spd);
    }

    public void LevelUp()
    {
        if(home.pd.Essence > mb.stats.level*100)
        {
            home.pd.Essence -= mb.stats.level * 100;
            mb.stats.level++;
            if(Random.Range(0.0f,1.0f) > 0.5f) { mb.stats.maxHp++; }
            if(Random.Range(0.0f,1.0f) > 0.5f) { mb.stats.atk++; }
            if(Random.Range(0.0f,1.0f) > 0.5f) { mb.stats.spd++; }
        }
    }

    private float GetTraitMultiplier(StatTypes statType, int originalStat)
    {

        List<Trait> tempTraits = new List<Trait>();

        #region adding character traits
        //job trait
        switch (mb.jobTraitIndex)
        {
            case 0:
                tempTraits.Add(home.tl.traits[0]);
                break;
            case 1:
                tempTraits.Add(home.tl.traits[1]);
                break;
            case 2:
                tempTraits.Add(home.tl.traits[2]);
                break;
            case 3:
                tempTraits.Add(home.tl.traits[6]);
                break;
        }

        //race trait
        switch (mb.raceTraitIndex)
        {
            case 0:
                tempTraits.Add(home.tl.traits[3]);
                break;
            case 1:
                tempTraits.Add(home.tl.traits[4]);
                break;
            case 2:
                tempTraits.Add(home.tl.traits[5]);
                break;
        }
        #endregion

        float oStat = originalStat;
        float cumultiveMultiplier = 0;
        foreach (Trait trait in tempTraits) 
        { 
            foreach (StatAffector sa in trait.affectedStats)
            {
                if (sa.stat == statType) { cumultiveMultiplier += sa.multiplier; }
            }
        }

        if (cumultiveMultiplier == 0) { return oStat; } else { return oStat * cumultiveMultiplier; }
    }
}
