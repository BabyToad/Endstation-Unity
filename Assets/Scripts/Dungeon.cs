using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{


    public void StartExpedition()
    {
        MasterSingleton.Instance.UIManger.ExpeditionUI(true);
    }

    public void Explore()
    {
        int roll = 0;
        int i = Random.Range(0, 3);
        if (i == 0)
        {
            roll = MasterSingleton.Instance.Guild.SelectedExplorer.RollDice(MasterSingleton.Instance.Guild.SelectedExplorer.Insight);
        }
        if (i == 1)
        {
            roll = MasterSingleton.Instance.Guild.SelectedExplorer.RollDice(MasterSingleton.Instance.Guild.SelectedExplorer.Prowess);
        }
        if (i == 3)
        {
            roll = MasterSingleton.Instance.Guild.SelectedExplorer.RollDice(MasterSingleton.Instance.Guild.SelectedExplorer.Resolve);
        }
        Debug.Log("Rolled a " + roll);
        if (roll <= 3)
        {
            Debug.Log("Expedition Fail. Loose Health and gain Stress");
            MasterSingleton.Instance.Guild.SelectedExplorer.AddHealth(-1);
            MasterSingleton.Instance.Guild.SelectedExplorer.AddStress(1);
        }
        else if (roll <= 5)
        {
            Debug.Log("Expedition Partial. Gain Stress. Gain Cred.");
            MasterSingleton.Instance.Guild.SelectedExplorer.AddStress(1);
            MasterSingleton.Instance.Guild.Cred++;
        }
        else if (roll == 6)
        {
            Debug.Log("Expedition Sucess. Gain Cred.");
            MasterSingleton.Instance.Guild.Cred++;
        }

    }

    void Explore(Explorer explorer)
    {
        int roll = explorer.RollDice(explorer.Insight);
        Debug.Log("Rolled a " + roll);
        if (roll <= 3)
        {
            Debug.Log("Expedition Fail. Loose Health and gain Stress");
            explorer.AddHealth(-1);
            explorer.AddStress(1);
        }
        else if (roll <= 5)
        {
            Debug.Log("Expedition Partial. Gain Stress. Gain Cred.");
            explorer.AddStress(1);
            MasterSingleton.Instance.Guild.Cred++;
        }
        else if (roll == 6)
        {
            Debug.Log("Expedition Sucess. Gain Cred.");
            MasterSingleton.Instance.Guild.Cred++; ;
        }

    }
    
}
