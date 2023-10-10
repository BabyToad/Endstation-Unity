using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhaseManager : MonoBehaviour
{
    public enum GameplayPhase
    {
        Downtime,
        Expedition
    }
    [SerializeField]
    GameplayPhase _phase;

    Animator animator;
    AnimatorStateInfo _stateInfo;


    public AnimatorStateInfo StateInfo { get => _stateInfo; set => _stateInfo = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public GameplayPhase Phase { get => _phase; set => _phase = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeState(GameplayPhase phase)
    {
        if (phase.Equals(GameplayPhase.Downtime))
        {
            animator.SetTrigger("Downtime");
        }
        if (phase.Equals(GameplayPhase.Expedition))
        {
            animator.SetTrigger("Expedition");
        }
    }

    public void StartExpedition()
    {
        ChangeState(GameplayPhase.Expedition);
    }
    public void StartDowntime()
    {
        ChangeState(GameplayPhase.Downtime);
    }
}
