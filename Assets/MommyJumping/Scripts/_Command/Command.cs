using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command 
{
   public abstract void Execute(Animator ani);
}

public class PerfomJump:Command
{
    public override  void Execute(Animator ani)
    {
        ani.SetTrigger("isJumping");
    }
}

public class MoveToForward : Command
{
    public override void Execute(Animator ani)
    {
        ani.SetTrigger("isWalking");
    }
}

public class PerfomKick : Command
{
    public override void Execute(Animator ani)
    {
        ani.SetTrigger("isKicking");
    }
}

public class DoNotThing : Command
{
    public override void Execute(Animator ani)
    {

    }
}
