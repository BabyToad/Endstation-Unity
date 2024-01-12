using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]

public class Explorer
{
    Guild _guild;

    [SerializeField]
    string _name;
    [SerializeField]
    int _health;
    [SerializeField]
    int _stress;
    [SerializeField]
    int _insight;
    [SerializeField]
    int _prowess;
    [SerializeField]
    int _resolve;
    [SerializeField]
    bool _exhausted;

    public enum Attribute
    {
        Insight,
        Prowess,
        Resolve
    }

    [SerializeField]
    GameObject _ui;
    ExplorerCanvas _explorerCanvas;

    [SerializeField]
    GameObject _uiTemplate;


    public Explorer(string name, int health, int insight, int prowess, int resolve, Guild guild)
    {
        Name = name;
        Health = health;
        Insight = insight;
        Prowess = prowess;
        Resolve = resolve;
        _guild = guild;
        AddExplorerToUI();
    }

    public int Insight { get => _insight; set => _insight = value; }
    public int Prowess { get => _prowess; set => _prowess = value; }
    public int Resolve { get => _resolve; set => _resolve = value; }
    public int Health { get => _health; set => _health = value; }
    public int Stress { get => _stress; set => _stress = value; }
    public string Name { get => _name; set => _name = value; }
    public ExplorerCanvas ExplorerCanvas { get => _explorerCanvas; set => _explorerCanvas = value; }
    public bool Exhausted { get => _exhausted; set => _exhausted = value; }

    public void AddExplorerToUI()
    {
        _uiTemplate = Resources.Load<GameObject>("Explorer Canvas");
        _ui = GameObject.Instantiate(_uiTemplate, GameObject.Find("Explorer Canvas").transform);
        _explorerCanvas = _ui.GetComponent<ExplorerCanvas>();

        _explorerCanvas.SetName(Name);
        _explorerCanvas.SetHealth(Health);
        _explorerCanvas.SetStress(Stress);
        _explorerCanvas.SetInsight(Insight.ToString());
        _explorerCanvas.SetProwess(Prowess.ToString());
        _explorerCanvas.SetResolve(Resolve.ToString());

        Toggle toggle = _explorerCanvas.GetComponent<Toggle>();

        _guild.RosterTG.RegisterToggle(toggle);
        toggle.group = _guild.RosterTG;

        toggle.onValueChanged.AddListener(SelectExplorer);

       
    }

    public int RollDice(int dice)
    {
        if (_stress >= 9)
        {
            dice--;
        }
        int result = 0;
        for (int i = 0; i < dice; i++)
        {
            int roll = Random.Range(1, 7);
            if (roll > result)
            {
                result = roll;
            }
        }

        if (dice <= 0)
        {
            int roll1 = Random.Range(1, 7);
            int roll2 = Random.Range(1, 7);

            result = Mathf.Min(roll1, roll2);
        }
        Debug.Log("Dice: " + result);
        return result;
    }

    public int RollDice(Attribute attribute)
    {
        if (attribute == Attribute.Insight)
        {
            return RollDice(_insight);
        }
        if (attribute == Attribute.Prowess)
        {
            return RollDice(_prowess);
        }
        if (attribute == Attribute.Resolve)
        {
            return RollDice(_resolve);
        }
        else
        {
            return 0;
        }
    }

    public void AddHealth(int health)
    {
        Health += health;
        Health = Mathf.Clamp(Health, 0, 5);
        _explorerCanvas.SetHealth(Health);
    }
    public void AddStress(int stress)
    {
        Stress += stress;
        Stress = Mathf.Clamp(Stress, 0, 9);
        _explorerCanvas.SetStress(Stress);
    }

    public void SelectExplorer(bool value)
    {
        if (value)
        {
            _guild.SelectedExplorer = this;
        }
        else
        {
            if (_guild.SelectedExplorer == this)
            {
                _guild.SelectedExplorer = null;
            }
        }
    }

    public int GetLowestAttributeStat()
    {
        return  Mathf.Min(Resolve, Insight, Prowess); ;
    }

    public void Exhaust()
    {
        _exhausted = true;
        ExplorerCanvas.DisplayExhaustion(true);
    }

    public void Rest()
    {
        _exhausted = false;
        ExplorerCanvas.DisplayExhaustion(false);
    }
}
