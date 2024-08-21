
/// <summary>
/// ObjectPooling을 하는 객체가 구현해야 하는 인터페이스
/// 
/// YWS : 2024.07.18
/// </summary>
public interface ISave
{
    public void Save(string path);
    public void Load(string path);
}
