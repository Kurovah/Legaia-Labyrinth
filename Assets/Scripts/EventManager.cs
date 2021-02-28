using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public RuinManager ruinManager;
    public EventList el;
    public List<RuinEvent> ruinEvents = new List<RuinEvent>();
    public int eventIndex;
    public float eventTimer;
    bool startScrolling = true;
    public GameObject textbox;
    public Text textboxText, timerText;
    List<string> dialouge = new List<string>();
    public List<Sprite> eventSprites;
    bool dialougeOpen = false ,giveEssence, healAll;
    int dialogueIndex;
    // Start is called before the first frame update
    void Start()
    {
        eventTimer = 10;
        eventIndex = 0;
        startScrolling = true;


        //for testing
        //ruinEvents.Add(el.events[2]);
        //ruinEvents.Add(el.events[0]);
        //ruinEvents.Add(el.events[1]);

        //add events
        for(int i = 0; i< Random.Range(3, 7); i++)
        {
            float spawnChance = Random.value;
            if(spawnChance < 0.5f)
            {
                ruinEvents.Add(el.events[0]);
            } else if(spawnChance < 0.95f)
            {
                ruinEvents.Add(el.events[2]);
            } else
            {
                ruinEvents.Add(el.events[1]);
            }
            
        }

        textbox.SetActive(dialougeOpen);
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Next event in:"+eventTimer;
        //scrollthrough events until done
        if (startScrolling)
        {
            StartCoroutine(GotoNextEvent());
        }


        if (dialougeOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dialogueIndex++;
                if (dialogueIndex < dialouge.Count)
                {
                    textboxText.text = dialouge[dialogueIndex];
                } else
                {
                    dialougeOpen = false;
                    textbox.SetActive(false);
                    if (giveEssence) { ruinManager.collectedEssence += 100; }
                    if (healAll) {
                        ruinManager.bm.HealAllAllies(2);
                    }
                    FinishEvent();
                }
            }
        }
    }

    IEnumerator GotoNextEvent()
    {
        startScrolling = false;
        Debug.Log("started coroutine");
        while(eventTimer > 0)
        {
            eventTimer -= 1f;
            yield return new WaitForSeconds(1);
        }

        ResolveEvent(eventIndex);

        Debug.Log("event Timer paused");
    }

    public void ResolveEvent(int index)
    {
        RuinEventType ret = ruinEvents[index].ruinEventType;
        RuinEventEffect ree = ruinEvents[index].eventEffect;
        switch (ret)
        {
            case RuinEventType.Battle:
                ruinManager.bm.StartBattle();
                break;
            case RuinEventType.AffectAll:
                switch (ree)
                {
                    case RuinEventEffect.HealAll:
                        dialouge.Clear();
                        dialouge.Add(ruinEvents[index].startText); 
                        dialouge.Add(ruinEvents[index].passText);
                        healAll = true;
                        StartDialouge();
                        break;
                }
                break;
            case RuinEventType.CheckClass:
                dialouge.Clear();
                dialouge.Add(ruinEvents[index].startText);
                if (LookForClass(ruinEvents[index].targetClass.ToString())) 
                { 
                    dialouge.Add(ruinEvents[index].passText);
                    giveEssence = true;
                } else { 
                    dialouge.Add(ruinEvents[index].failText);
                    giveEssence = false;
                }
                StartDialouge();
                break;
        }
    }

    public bool LookForClass(string className)
    {
        foreach (BattleEntity be in ruinManager.bm.allies)
        {
            if (be.HasTrait(className)) { return true; }
        }

        return false;
    }

    public void FinishEvent()
    {
        giveEssence = false;
        healAll = false;
        if(eventIndex == ruinEvents.Count - 1)
        {
            ruinManager.Win();
        } else
        {
            eventIndex++;
            eventTimer = 10;
            startScrolling = true;
        }
    }

    private void StartDialouge()
    {
        dialougeOpen = true;
        dialogueIndex = 0;
        textbox.SetActive(dialougeOpen);
        textboxText.text = dialouge[dialogueIndex];
    }
}
