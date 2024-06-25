using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trait", menuName = "ScriptableObjects/Trait", order = 1)]
[System.Serializable]
public class Trait: ScriptableObject
{
    public int id;
    public string _name;
    public Sprite icon;
    public string description;
    
    //Action Modifiers
    [System.Serializable]
    public class StatChangeModifiers
    {
        public int _hp;
        public int _stress;
        public int _scrap;
        public int _cred;
    }

    [SerializeField] public StatChangeModifiers statChangeModifiers;

    public Trait()
    {
        id = -1;
    }
}
