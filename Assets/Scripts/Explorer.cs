using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using System.Linq;

[System.Serializable]

public class Explorer
{
    Guild _guild;

    [SerializeField] string _name;
    [SerializeField] int _health;
    [SerializeField] int _stress;
    [SerializeField] int _insight;
    [SerializeField] int _prowess;
    [SerializeField] int _resolve;
    [SerializeField] List<Trait> _traits =  new();
    [SerializeField] int _experience;
    [SerializeField] bool _exhausted;
    [SerializeField] bool _isTrauma;
    [SerializeField] bool _isInjured;
    [SerializeField] bool _hasAdvancement;

    public enum Attribute
    {
        Insight,
        Prowess,
        Resolve
    }

    [SerializeField] GameObject _ui;
    ExplorerCanvas _explorerCanvas;

    [SerializeField] GameObject _uiTemplate;


    public Explorer(string name, int health, int insight, int prowess, int resolve, Guild guild)
    {
        Name = name;
        Health = health;
        Insight = insight;
        Prowess = prowess;
        Resolve = resolve;
        Experience = 0;
        _guild = guild;
        AddExplorerToUI();

    }

    public int Insight { get => _insight; set => _insight = value; }
    public int Prowess { get => _prowess; set => _prowess = value; }
    public int Resolve { get => _resolve; set => _resolve = value; }
    public int Experience { get => _experience; set => _experience = value; }
    public int Health { get => _health; set => _health = value; }
    public int Stress { get => _stress; set => _stress = value; }
    public string Name { get => _name; set => _name = value; }
    public ExplorerCanvas ExplorerCanvas { get => _explorerCanvas; set => _explorerCanvas = value; }
    public bool Exhausted { get => _exhausted; set => _exhausted = value; }
    public bool IsTrauma { get => _isTrauma; set => _isTrauma = value; }
    public bool IsInjured { get => _isInjured; set => _isInjured = value; }
    public bool HasAdvancement { get => _hasAdvancement; set => _hasAdvancement = value; }
    public List<Trait> Traits { get => _traits; set => _traits = value; }

    public void AddExplorerToUI()
    {
        _uiTemplate = Resources.Load<GameObject>("Explorer Canvas");
        _ui = GameObject.Instantiate(_uiTemplate, GameObject.Find("Explorer Canvas").transform);
        _explorerCanvas = _ui.GetComponent<ExplorerCanvas>();

        _explorerCanvas.ReferenceExplorer(this);
        _explorerCanvas.SetName(Name);
        _explorerCanvas.SetHealth(Health);
        _explorerCanvas.SetStress(Stress);
        _explorerCanvas.SetInsight(Insight, IsInjured, IsTrauma);
        _explorerCanvas.SetProwess(Prowess, IsInjured, IsTrauma);
        _explorerCanvas.SetResolve(Resolve, IsInjured, IsTrauma);
        _explorerCanvas.SetTraits(_traits.ToArray());

        Toggle toggle = _explorerCanvas.GetComponent<Toggle>();

        _guild.RosterTG.RegisterToggle(toggle);
        //toggle.group = _guild.RosterTG;

        //toggle.onValueChanged.AddListener(SelectExplorer);

        _explorerCanvas.AdvancementButtons[0].onClick.AddListener(AdvanceInsight);
        _explorerCanvas.AdvancementButtons[1].onClick.AddListener(AdvanceProwess);
        _explorerCanvas.AdvancementButtons[2].onClick.AddListener(AdvanceResolve);

    }

    void UpdateExplorerCanvasStats()
    {
        int insight, prowess, resolve;
        insight = ApplyDicePenaltyAndReturn(Insight);
        prowess = ApplyDicePenaltyAndReturn(Prowess); 
        resolve = ApplyDicePenaltyAndReturn(Resolve); 


        _explorerCanvas.SetInsight(insight, IsInjured, IsTrauma);
        _explorerCanvas.SetProwess(prowess, IsInjured, IsTrauma);
        _explorerCanvas.SetResolve(resolve, IsInjured, IsTrauma);
        _explorerCanvas.SetTraits(_traits.ToArray());
        _explorerCanvas.ShowAdvancementButtons(_hasAdvancement);
    }

    

    int ApplyDicePenaltyAndReturn(int stat)
    {
        if (_isTrauma)
        {
            stat--;
        }
        if (_isInjured)
        {
            stat--;
        }
        return Mathf.Clamp(stat, 0, 10000);
    }

    public int RollDice(int dice)
    {
        if (_isTrauma)
        {
            dice--;
        }
        if (_isInjured)
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
        int maxHealth = 3;
        foreach (Trait trait in Traits)
        {
            if (trait.statChangeModifiers._hp == 0)
            {
                return;
            }
            //you should only be able to affect damage taken, not heal faster
            if (health < 0)
            {
                health += trait.statChangeModifiers._hp;
                health = Mathf.Clamp(health, 0, maxHealth);
                Debug.Log("Modified health change by " + trait.statChangeModifiers._hp + " due to Trait " + trait.name);

            }
        }

        Health += health;
        
        Health = Mathf.Clamp(Health, 0, maxHealth);
        
        if (_health == 0)
        {
            _isInjured = true;
        }
        else
        {
            _isInjured = false;
        }
        UpdateExplorerCanvasStats();
        _explorerCanvas.SetHealth(Health);
    }
    public void AddStress(int stress)
    {
        Debug.Log("Stress was+ "+ Stress + ". Added "+ stress);

        int maxStress = 9;
        Debug.Log("Stress was+ " + Stress + ". Added " + stress);
        if (Traits != null)
        {
            foreach (Trait trait in Traits)
            {
                if (trait.statChangeModifiers._stress == 0)
                {
                    return;
                }
                //if you would be gaining stress, you should not be able to reduce it below 0
                if (stress >= 0)
                {
                    stress += trait.statChangeModifiers._stress;
                    stress = Mathf.Clamp(stress, 0, maxStress);
                }
                else
                {
                    stress += trait.statChangeModifiers._stress;
                }
                Debug.Log("Modified stress change by " + trait.statChangeModifiers._stress + " due to Trait " + trait.name);
            }
        }
        
        Stress += stress;
        Stress = Mathf.Clamp(Stress, 0, maxStress);
        
        if (_stress == maxStress)
        {
            _isTrauma = true;
        }
        else
        {
            _isTrauma = false;
        }
        UpdateExplorerCanvasStats();
        _explorerCanvas.SetStress(Stress);
    }

    public void AddStress(int stress, bool updateDisplay)
    {
        int maxStress = 9;
        Debug.Log("Stress was+ " + Stress + ". Added " + stress);
        if (Traits != null)
        {
            foreach (Trait trait in Traits)
            {
                if (trait.statChangeModifiers._stress == 0)
                {
                    return;
                }
                //if you would be gaining stress, you should not be able to reduce it below 0
                if (stress >= 0)
                {
                    stress += trait.statChangeModifiers._stress;
                    stress = Mathf.Clamp(stress, 0, maxStress);
                }
                else
                {
                    stress += trait.statChangeModifiers._stress;
                }
                Debug.Log("Modified stress change by " + trait.statChangeModifiers._stress + " due to Trait " + trait.name);
            }
        }
        Stress += stress;
        
        Stress = Mathf.Clamp(Stress, 0, maxStress);

        if (_stress == maxStress)
        {
            _isTrauma = true;
        }
        else
        {
            _isTrauma = false;
        }
        if (updateDisplay)
        {
            UpdateExplorerCanvasStats();
            _explorerCanvas.SetStress(Stress);
        }
        
    }

    public void AddExperience(int exp)
    {
        Experience += exp;
        int maxExperience = 9;
        Experience = Mathf.Clamp(Experience, 0, maxExperience);

        if (_experience == maxExperience)
        {
            //implement level up
            _hasAdvancement = true;
        }
        else
        {
            _hasAdvancement = false;
        }
        UpdateExplorerCanvasStats();
        _explorerCanvas.SetXP(exp);
    }

    public void AdvanceInsight()
    {
        _insight++;
        _experience = 0;
        _hasAdvancement = false;
        UpdateExplorerCanvasStats();
    }

    public void AdvanceProwess()
    {
        _prowess++;
        _experience = 0;
        _hasAdvancement = false;

        UpdateExplorerCanvasStats();
    }
    public void AdvanceResolve()
    {
        _resolve++;
        _experience = 0;
        _hasAdvancement = false;

        UpdateExplorerCanvasStats();

    }

    public void SelectExplorer(bool value)
    {
        if (value)
        {
            _guild.SelectedExplorers.Add(this);
            _explorerCanvas.GetComponent<Toggle>().isOn = value;
        }
        else
        {
            _explorerCanvas.GetComponent<Toggle>().isOn = value;

            if (_guild.SelectedExplorers.Contains(this))
            {
                _guild.SelectedExplorers.Remove(this);
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

    public void AddTrait(Trait trait)
    { 
        Traits.Add(trait);
    }

    public void RemoveTrait(Trait trait)
    {
        Traits.Remove(trait);
    }
}
