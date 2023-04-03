using Util;

public enum SoundGroupType
{
    Master,
    Bgm,
    Sfx,
}

public class SoundGroup : Singleton<SoundGroup>
{
    public string Master = "Master";
    public string Bgm = "Bgm";
    public string Sfx = "Sfx";

    public string GetSound(SoundGroupType type) => type switch
    {
        SoundGroupType.Master => Master,
        SoundGroupType.Bgm => Bgm,
        SoundGroupType.Sfx => Sfx,
        _ => null,
    };
}
