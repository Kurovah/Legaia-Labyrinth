using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeBaseBehaviour : MonoBehaviour
{
    public List<GameObject> tabGroups;
    public List<ShowMirageStats> mirageTabs;
    public TraitList tl;
    public PartyData pd;
    public Text EssenceCount;
    // Start is called before the first frame update
    void Start()
    {
        pd = SaveSystem.GetPartyData();
        ChangeTab(0);
        int i = 0;
        foreach(ShowMirageStats show in mirageTabs)
        {
            show.mb = pd.members[i];
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        EssenceCount.text = "E:"+pd.Essence.ToString();
    }

    public void ChangeTab(int tab)
    {
        int i = 0;
        foreach(GameObject go in tabGroups)
        {
            go.SetActive(false);
            if(i == tab)
            {
                go.SetActive(true);
            }
            i++;
        }

        Debug.Log("Tab" + tab);
    }

    public void GoToRuins()
    {
        SaveSystem.SavePartyData(pd);
        SceneManager.LoadScene("Ruins");
    }

    public void BackToMenu()
    {
        SaveSystem.SavePartyData(pd);
        SceneManager.LoadScene("MainMenu");
    }
}
