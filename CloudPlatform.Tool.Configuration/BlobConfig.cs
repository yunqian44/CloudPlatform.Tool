namespace CloudPlatform.Tool.Configuration;
public interface IBlobConfig
{
    GeneralSettings GeneralSettings { get; set; }

    void LoadFromConfig(string config);
}

public class BlobConfig : IBlobConfig
{
    public GeneralSettings GeneralSettings { get; set; }

    public void LoadFromConfig(string config)
    {
        GeneralSettings = config.FromJson<GeneralSettings>();
    }
}