using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relationship : Trait
{
    [SerializeField] public Explorer _explorer;
    [SerializeField] public int _strength;

    public Relationship(Explorer exp, int strength)
    {
        _explorer = exp;
        _strength = strength;
        name = "Relationship: " + _explorer.Name;
    }
}
