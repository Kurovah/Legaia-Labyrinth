using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RuinManager : MonoBehaviour
{
    public Transform HUDBar, characterPlace;
    public BattleManager bm;
    public EventManager em;
    public GameObject statusBarPrefab, characterPrefab;
    public TraitList tl;
    public bool paused;
    public int collectedEssence,healCharges,reviveCharges;
    public Text essenceCounter, healChargeText, reviveChargeText;
    public GameObject loseBoard, winBoard;
    public List<Sprite> headSprites, bodySprites,enemySprites;
    // Start is called before the first frame update
    void Start()
    {
        healCharges = 3;
        reviveCharges = 1;
        loseBoard.SetActive(false);
        winBoard.SetActive(false);
        int i = 0;
        PartyData pd = SaveSystem.GetPartyData();
        Debug.Log("loading " +pd.members.Count+ " members");
        foreach(MemberData md in pd.members)
        {

            GameObject mirage = Instantiate(characterPrefab, characterPlace);
            mirage.transform.localPosition = new Vector2((i - 1)*2,1 - i);
            SpriteHandler msh = mirage.GetComponent<SpriteHandler>();
            BattleEntity be = mirage.GetComponent<BattleEntity>();
            be.stats = md.stats;
            #region adding character traits
            //job trait
            switch (md.jobTraitIndex)
            {
                case 0:
                    be.traits.Add(tl.traits[0]);
                    be.attackMethod = AttackMethod.attackSingle;
                    msh.body.sprite = bodySprites[0];
                    break;
                case 1:
                    be.traits.Add(tl.traits[1]);
                    be.attackMethod = AttackMethod.Heal;
                    msh.body.sprite = bodySprites[1];
                    break;
                case 2:
                    be.traits.Add(tl.traits[2]);
                    be.attackMethod = AttackMethod.attackAll;
                    msh.body.sprite = bodySprites[2];
                    break;
                case 3:
                    be.traits.Add(tl.traits[6]);
                    be.attackMethod = AttackMethod.attackSingle; 
                    msh.body.sprite = bodySprites[3];
                    break;
            }

            //race trait
            switch (md.raceTraitIndex)
            {
                case 0:
                    be.traits.Add(tl.traits[3]);
                    msh.head.sprite = headSprites[0];
                    break;
                case 1:
                    be.traits.Add(tl.traits[4]);
                    msh.head.sprite = headSprites[1];
                    break;
                case 2:
                    be.traits.Add(tl.traits[5]);
                    msh.head.sprite = headSprites[2];
                    break;
            }
            #endregion
            be.hp = be.GetTraitMultiplier(StatTypes.maxHp, be.stats.maxHp);

            GameObject barSet = Instantiate(statusBarPrefab, HUDBar);
            StatHandler sh = barSet.GetComponent<StatHandler>();
            be.timeScaler = sh.timeScaler;
            be.hpScaler = sh.healthScaler;

            be.hpScaler.maxValue = be.GetTraitMultiplier(StatTypes.maxHp,be.stats.maxHp);
            be.timeScaler.maxValue = 5;
            sh.barText.text = md.memberName;
            i++;
            be.bm = bm;


            
            
            bm.allies.Add(be);
        }

    }

    // Update is called once per frame
    void Update()
    {
        essenceCounter.text = "Essence:"+collectedEssence.ToString();
        reviveChargeText.text = "Revives Left:" + reviveCharges;
        healChargeText.text = "Heals Left:" +healCharges;
        if (loseBoard.activeSelf || winBoard.activeSelf && Input.GetMouseButtonDown(0))
        {
            BackToBase();
        }
    }

    public void BackToBase()
    {
        PartyData pd = SaveSystem.GetPartyData();
        pd.Essence += collectedEssence;
        SaveSystem.SavePartyData(pd);
        SceneManager.LoadScene("HomeBase");
    }

    public void Lose()
    {
        collectedEssence = collectedEssence/2;
        loseBoard.SetActive(true);
    }

    public void Win()
    {
        winBoard.SetActive(true);
    }

    public void TryHealAll()
    {
        if(healCharges > 0)
        {
            healCharges--;
            bm.HealAllAllies(1);
        }
    }

    public void TryReviveAll()
    {
        if (reviveCharges > 0)
        {
            reviveCharges--;
            bm.ReviveAllies();
        }
    }
}
