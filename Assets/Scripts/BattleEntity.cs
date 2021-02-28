using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : MonoBehaviour
{
    public Stats stats;
    SpriteHandler sh;
    public GameObject healFx, hitFx;
    public bool startDecrease,alive,ally,fighting,givenEssence;
    public float waitTime;
    public float hp;
    public BarScaler hpScaler,timeScaler;
    public BattleManager bm;
    public List<Trait> traits;
    Coroutine timerRoutine;
    public AttackMethod attackMethod = AttackMethod.attackSingle;

    // Start is called before the first frame update
    void Awake()
    {
        sh = GetComponent<SpriteHandler>();
        alive = true;
        waitTime = 5;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAlive();
        if (alive)
        {
            if (sh.head != null)
            {
                sh.head.color = Color.white;
            }
            if (sh.body != null)
            {
                sh.body.color = Color.white;
            }
            if (waitTime <= 0)
            {
                switch (attackMethod)
                {
                    case AttackMethod.attackSingle:
                        if (ally) { Attack(bm.enemies); } else { Attack(bm.allies); }
                        break;
                    case AttackMethod.attackAll:
                        if (ally) { AttackAll(bm.enemies); } else { AttackAll(bm.allies); }
                        break;
                    case AttackMethod.Heal:
                        if (ally) { HealAllies(bm.allies); } else { HealAllies(bm.enemies); }
                        break;
                }
                    
                
                
                
                waitTime = 5;
            }
            else
            {
                if (startDecrease && fighting)
                {
                    timerRoutine = StartCoroutine(IncreaseWait());
                }
            }
        }
        else
        {
            if(sh.head != null)
            {
                sh.head.color = Color.gray;
            }
            if (sh.body != null)
            {
                sh.body.color = Color.gray;
            }
        }
        UpdateBars();
    }

    public void Attack(List<BattleEntity> targetside)
    {
        bool haveTarget = false;
        List<BattleEntity> search = new List<BattleEntity>();
        foreach(BattleEntity be in targetside)
        {
            search.Add(be);
        }

        BattleEntity target = null;
        while (!haveTarget && search.Count > 0) {
            target = search[Random.Range(0,search.Count)];
            if (target.alive) { haveTarget = true; }
            search.Remove(target);
        }

        if(target != null) { target.hp -= GetTraitMultiplier(StatTypes.atk,stats.atk); Instantiate(hitFx, target.gameObject.transform.position - Vector3.forward * 0.2f, Quaternion.identity); }
    }

    public void AttackAll(List<BattleEntity> targetside)
    {
        foreach (BattleEntity be in targetside)
        {
            if (be.alive) { be.hp -= GetTraitMultiplier(StatTypes.atk, stats.atk); Instantiate(hitFx, be.gameObject.transform.position - Vector3.forward*0.2f, Quaternion.identity); }
        }
    }

    public void HealAllies(List<BattleEntity> targetside)
    {
        BattleEntity be = targetside[0];
        foreach(BattleEntity sbe in targetside)
        {
            if(sbe.hp < be.hp && sbe.alive) { be = sbe; }
        }

        be.Heal((int)GetTraitMultiplier(StatTypes.atk, stats.atk));
    }

    public void ApplyStats(Stats _stats, int levelMod)
    {
        int lvl = Random.Range(levelMod, levelMod+2);
        stats.level = lvl;
        hp = _stats.maxHp * lvl;
        stats.maxHp = _stats.maxHp * lvl;
        stats.spd = _stats.spd * lvl;
    }

    IEnumerator IncreaseWait()
    {
        startDecrease = false;
        while(waitTime > 0)
        {
            waitTime -= GetTraitMultiplier(StatTypes.spd, stats.spd);
            yield return new WaitForSeconds(1.0f);
        }
        startDecrease = true;
    }

    void UpdateBars()
    {
        hpScaler.value = hp;
        timeScaler.value = waitTime;
    }

    public void StopFighting()
    {
        StopCoroutine(timerRoutine);
        fighting = false;
        waitTime = 5;
    }


    public void CheckAlive()
    {
        if(hp <= 0)
        {
            hp = 0;
            alive = false;
            if (!ally&& !givenEssence)
            {
                givenEssence = true;
                bm.rm.collectedEssence += Random.Range(50, 100);
            }
        }
    }

    public void Heal(int heal)
    {
        hp += heal;
        if(hp > GetTraitMultiplier(StatTypes.maxHp, stats.maxHp)) { hp = GetTraitMultiplier(StatTypes.maxHp, stats.maxHp); }
        Instantiate(healFx, gameObject.transform.position - Vector3.forward * 0.2f, Quaternion.identity);
    }

    public bool HasTrait(string name)
    {
        foreach(Trait t in traits)
        {
            if(t.name == name) { return true; }
        }
        return false;
    }

    public float GetTraitMultiplier(StatTypes statType, int originalStat)
    {
        float oStat = originalStat;
        float cumultiveMultiplier = 0;
        foreach(Trait trait in traits)
        {
            foreach(StatAffector sa in trait.affectedStats)
            {
                if(sa.stat == statType) { cumultiveMultiplier += sa.multiplier; }
            }
        }

        if(cumultiveMultiplier == 0) { return oStat; } else { return oStat * cumultiveMultiplier; }
    }


}
