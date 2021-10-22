using System.Threading.Tasks;

namespace TestMasterInfrastructure.Wrappers
{
    public interface IConfigWrapper
    {
        string GetAppSettings(string settingName);
        T GetAppSettings<T>(string settingName) where T : new();
    }
}