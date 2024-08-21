/// <summary>
/// UpdateManager를 통해 Update를 수행할 객체가 구현해야 하는 인터페이스
/// 
/// YWS : 2024.06.16
/// </summary>
public interface IUpdateable
{
    public void UpdateWork();
    public void FixedUpdateWork();
    public void LateUpdateWork();
}
