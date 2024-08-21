using UnityEngine;

/// <summary>
/// UI용 빌보드 클래스
/// 
/// YWS : 2024.07.23
/// </summary>
public class BillBoard : MonoBehaviour, IUpdateable
{
    private void OnEnable()
    {
        UpdateManager.OnSubscribe(this,true, false, false);
    }

    private void OnDisable()
    {
        UpdateManager.OffSubscribe(this, true, false, false);
    }

    public void UpdateWork()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    public void FixedUpdateWork()
    {
    }

    public void LateUpdateWork()
    {
    }
}
