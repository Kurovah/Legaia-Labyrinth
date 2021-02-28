using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public RuinManager rm;
    public Transform enemyPlace;
    public EnemyList el;
    public GameObject enemyPrefab,battlestartFX;
    public List<BattleEntity> allies = new List<BattleEntity>(), enemies = new List<BattleEntity>();
    bool battleInProgress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (battleInProgress)
        {
            if (checkAllDead(enemies)) { 
                WinBattle(); 
            }

            if (checkAllDead(allies))
            {
                LoseBattle();
            }
        }
    }

    public void StartBattle()
    {
        Instantiate(battlestartFX, Vector3.zero, Quaternion.identity);
        enemies.Clear();
        int enemyNum = Random.Range(1,4);
        for (int i = 0; i < enemyNum; i++)
        {
            GameObject en = Instantiate(enemyPrefab, enemyPlace);
            en.transform.localPosition = new Vector2((1-i)*2, 1 - i);

            int index = Random.Range(0, el.enemies.Count);
            BattleEntity be = en.GetComponent<BattleEntity>();

            int avglvl = 0;
            foreach (BattleEntity ae in allies)
            {
                avglvl += ae.stats.level;
            }

            avglvl /= allies.Count;
            be.ApplyStats(el.enemies[index].enemyStats, avglvl);
            en.GetComponent<SpriteHandler>().body.sprite = rm.enemySprites[index];

            be.hpScaler.maxValue = be.stats.maxHp;
            be.timeScaler.maxValue = 5;
            be.bm = this;
            
            enemies.Add(be);
        }

        foreach (BattleEntity be in allies)
        {
            if (be.alive) {
                be.startDecrease = be.fighting = true;
            }
        }

        foreach (BattleEntity be in enemies)
        {
            if (be.alive)
            {
                be.startDecrease = be.fighting = true;
            }
        }
        battleInProgress = true;
    }

    public void WinBattle()
    {
        battleInProgress = false;
        foreach (BattleEntity be in enemies)
        {
            be.startDecrease = false;
            be.StopFighting();
            Destroy(be.gameObject);
        }

        foreach (BattleEntity be in allies)
        {
            be.startDecrease = false;
            be.fighting = false;
            be.StopFighting();
        }
        rm.em.FinishEvent();
    }

    public void LoseBattle()
    {
        battleInProgress = false;
        foreach (BattleEntity be in enemies)
        {
            be.startDecrease = false;
            be.StopFighting();
            be.fighting = false;
        }

        foreach (BattleEntity be in allies)
        {
            be.startDecrease = false;
            be.fighting = false;
            be.StopFighting();
        }

        rm.Lose();
    }

    public bool checkAllDead(List<BattleEntity> list)
    {
        foreach (BattleEntity be in list)
        {
            if (be.alive) { return false;}
        }

        return true;

    }

    public void HealAllAllies(int amount)
    {
        foreach(BattleEntity be in allies)
        {
            if(be.alive)
            be.Heal(amount);
        }
    }

    public void ReviveAllies()
    {
        foreach (BattleEntity be in allies)
        {
            if (!be.alive) { 
                be.alive = true;
                be.hp = be.GetTraitMultiplier(StatTypes.maxHp,be.stats.maxHp);
            }
        }
    }
}
