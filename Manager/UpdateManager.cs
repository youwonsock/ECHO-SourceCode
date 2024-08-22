using System;
using UnityEngine;

/// <summary>
/// ����ȭ�� ���� Update, FixedUpdate, LateUpdate�� �ѹ��� �����ϴ� Ŭ����
/// �� ����� �ʿ�� ����!
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
