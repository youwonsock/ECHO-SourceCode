using System;
using UnityEngine;

/// <summary>
/// 최적화를 위해 Update, FixedUpdate, LateUpdate를 한번에 관리하는 클래스
/// 꼭 사용할 필요는 없음!
/// 
/// YWS : 2024.06.28
/// </summary>
public class UpdateManager : MonoBehaviour
{
    private static Action updateWorks;
    private static Action fixedUpdateWorks;
    private static Action lateUpdateWorks;



    private void Update()
    {
        updateWorks?.Invoke();
    }

    private void FixedUpdate()
    {
        fixedUpdateWorks?.Invoke();
    }

    private void LateUpdate()
    {
        lateUpdateWorks?.Invoke();
    }



    public static void OnSubscribe(IUpdateable updateable, bool update, bool fixedUpdate, bool lateUpdate)
    {
        if (update)
            updateWorks += updateable.UpdateWork;

        if (fixedUpdate)
            fixedUpdateWorks += updateable.FixedUpdateWork;

        if (lateUpdate)
            lateUpdateWorks += updateable.LateUpdateWork;
    }

    public static void OffSubscribe(IUpdateable updateable, bool update, bool fixedUpdate, bool lateUpdate)
    {
        if (update)
            updateWorks -= updateable.UpdateWork;

        if (fixedUpdate)
            fixedUpdateWorks -= updateable.FixedUpdateWork;

        if (lateUpdate)
            lateUpdateWorks -= updateable.LateUpdateWork;
    }
}
