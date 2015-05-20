using UnityEngine;
using ZFrame;

public class BehaviourTreeDemo : MonoBehaviour
{
    [Header("enemy in sight")] public bool enemyfound;
    [Header("will die")] public bool healthlow;
    [Header("hungry")] public bool hungry;
    [Header("need sleep")] public bool sleepy;
    [Header("Can attack enemy")] public bool stronger;

    private BehaviourTree tree = new BehaviourTree();

    private void Idle()
    {
        Debug.Log("Idle");
    }

    private void Cook()
    {
        Debug.Log("Cook");
    }

    private void Eat()
    {
        Debug.Log("Eat");
    }

    private void Sleep()
    {
        Debug.Log("Sleep");
    }

    private bool Flee()
    {
        Debug.Log("Flee");
        return true;
    }

    private void Attack()
    {
        Debug.Log("Attack");
    }

    private void Dead()
    {
        Debug.Log("Dead");
    }

    private bool CheckEnemy()
    {
        return enemyfound;
    }

    private bool CheckDead()
    {
        return healthlow;
    }


    private bool CheckCanAttack()
    {
        return stronger;
    }

    //idle, hungry, sleepy
    private int CheckIdle()
    {
        return hungry ? 1 : (sleepy ? 2 : 0);
    }

    private void Update()
    {
        tree.Selector(tree.Decorator(CheckDead, Dead),
            tree.Sequence(CheckEnemy, tree.Selector(tree.Decorator(CheckCanAttack, Attack), Flee)),
            tree.PrioritySelector(CheckIdle, Idle, tree.Parallel(Eat, Cook), Sleep)).Invoke();
    }
}