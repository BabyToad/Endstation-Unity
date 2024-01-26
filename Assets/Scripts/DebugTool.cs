using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTool : MonoBehaviour
{

    [SerializeField]
    bool _resetClocksOnPlay;
    
    [SerializeField]
    ProgressClock [] _progressClocks;
    // Start is called before the first frame update
    void Start()
    {
        _progressClocks = Resources.LoadAll<ProgressClock>("ProgressClocks");

        if (_resetClocksOnPlay)
        {
            ResetAllClocks();
        }
    }


    void ResetAllClocks()
    {
        for (int i = 0; i < _progressClocks.Length -1; i++)
        {
            _progressClocks[i].Fill = 0;
        }
    }

}
