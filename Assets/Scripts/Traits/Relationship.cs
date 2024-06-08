using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relationship : Trait
{
    [SerializeField] public Explorer _explorer;
    [SerializeField] public int _strength;
    [SerializeField] public int _maxStrength = 3;
    static public float chance = 0.15f;
    static public float decayChance = 0.1f;
    public Relationship(Explorer exp, int strength)
    {
        _explorer = exp;
        _strength = strength;
        name = "Relationship: " + _explorer.Name;
    }

    public void AddStrength(int strength)
    { 
        _strength += strength;
        _strength = Mathf.Clamp(_strength, -_maxStrength, _maxStrength);
        Debug.Log("New Relationship Strength: " + _strength);
    }
}
