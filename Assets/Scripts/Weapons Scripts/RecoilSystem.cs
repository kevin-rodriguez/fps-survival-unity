using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilSystem : MonoBehaviour
{
  [Header("Recoil Settings")]
  public Vector3 recoilKick = new Vector3(-2f, 0f, 0f);
  public float recoilDuration = 0.2f;

  private Coroutine recoilCoroutine;

  public void ApplyRecoil(Transform targetTransform)
  {
    if (recoilCoroutine != null)
    {
      StopCoroutine(recoilCoroutine);
    }

    recoilCoroutine = StartCoroutine(RecoilCoroutine(targetTransform));
  }

  private IEnumerator RecoilCoroutine(Transform targetTransform)
  {
    Quaternion originalRotation = targetTransform.localRotation;
    float elapsedTime = 0f;

    while (elapsedTime < recoilDuration)
    {
      float normalizedTime = elapsedTime / recoilDuration;
      Quaternion currentRecoilRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(recoilKick), normalizedTime);

      targetTransform.localRotation = originalRotation * currentRecoilRotation;

      elapsedTime += Time.deltaTime;
      yield return null;
    }
  }
}
