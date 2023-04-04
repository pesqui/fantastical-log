using Windows.Storage;
using FantasticalLog.Core.Contracts;

namespace FantasticalLog.Services;

public class FileService: IFileService
{
    private const string TEMP_DIR = "FantasticalLogTempDir";

    public FileService()
    {
    }

    public string TemporaryFolderPath
    {
        get {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return Path.Combine(localFolder.Path, TEMP_DIR);
        }
    }

    public async Task CreateTemporaryFolder()
    {
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        await localFolder.CreateFolderAsync(TEMP_DIR, CreationCollisionOption.ReplaceExisting);
    }
}
