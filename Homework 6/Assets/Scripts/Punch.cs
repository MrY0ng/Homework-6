using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : StateMachineBehaviour
{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Character.isLocked = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       Character.isLocked = false;
    }
}
