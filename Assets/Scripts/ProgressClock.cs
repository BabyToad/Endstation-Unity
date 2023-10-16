using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ProgressClock", menuName = "ScriptableObjects/ProgressClock", order = 1)]
public class ProgressClock : ScriptableObject
{
    [SerializeField]
    string _name;

    [SerializeField]
    bool _isCountdown;

    [SerializeField]
    Explorer.Attribute _actionAttribute;

    [SerializeField]
    int _segments;

    [SerializeField]
    int _fill = 0;

    [SerializeField]
    UnityEvent filled;
    SpriteRenderer _spriteRenderer;
    Sprite[] _sprites;

    [SerializeField]
    string _description;


    public int Segments { get => _segments; set => _segments = value; }
    public int Fill { get => _fill; set => _fill = value; }
    public Explorer.Attribute ActionAttribute { get => _actionAttribute; set => _actionAttribute = value; }
    public string Description { get => _description; set => _description = value; }
    public bool IsCountdown { get => _isCountdown; set => _isCountdown = value; }

    public void ChangeFill(int value)
    {
        _fill += value;
        _fill = Mathf.Clamp(_fill, 0, _segments);

        //_spriteRenderer.sprite = _sprites[_fill];
            
        if (_fill == _segments)
        {
            filled.Invoke();
        }
    }
}
