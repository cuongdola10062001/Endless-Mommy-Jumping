using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GameObject actor;
    Animator animator;
    Command keyW, keySpace, keyE, keyQ;

    List<Command> oldCommands = new List<Command>();
    Coroutine replayCoroutine;
    bool shouldStartReplay;
    bool isReplaying;

    private void Start()
    {
        keyW = new MoveToForward();
        keySpace= new PerfomJump();
        keyE = new PerfomKick();
        keyQ = new DoNotThing();

        animator = actor.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isReplaying)
        {
            HandleInput();
        }
        StartReplay();
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            keyW.Execute(animator);
            oldCommands.Add(keyW);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            keySpace.Execute(animator);
            oldCommands.Add(keySpace);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            keyE.Execute(animator);
            oldCommands.Add(keyE);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            keyQ.Execute(animator);
            oldCommands.Add(keyQ);

        }
    }
    void StartReplay()
    {
        if(shouldStartReplay&& oldCommands.Count > 0)
        {
            shouldStartReplay = false;
            if (replayCoroutine != null)
            {
                StopCoroutine(replayCoroutine);
            }
            replayCoroutine = StartCoroutine(ReplayCommands());
        }
    }

    IEnumerator ReplayCommands()
    {
        isReplaying = true;
        for(int i=0; i< oldCommands.Count; i++)
        {
            oldCommands[i].Execute(animator);
            yield return new WaitForSeconds(1f);
        }
        isReplaying = false;

    }
}
