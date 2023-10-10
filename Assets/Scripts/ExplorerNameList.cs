using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NameList", menuName = "ScriptableObjects/NameList", order = 1)]

public class ExplorerNameList : ScriptableObject
{
    [SerializeField]
    List<string> _names;

    public string GenerateName()
    {
        string name = _names[Random.Range(0, _names.Count - 1)];
        return name;
    }
}
