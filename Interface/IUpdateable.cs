/// <summary>
/// UpdateManager�� ���� Update�� ������ ��ü�� �����ؾ� �ϴ� �������̽�
/// 
/// YWS : 2024.06.16
/// </summary>
public interface IUpdateable
{
    public void UpdateWork();
    public void FixedUpdateWork();
    public void LateUpdateWork();
}
