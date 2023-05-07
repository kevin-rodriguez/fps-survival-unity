using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorHelper
{

  public static bool IsAnimationPlaying(Animator animator, string name)
  {
    return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
  }

}
