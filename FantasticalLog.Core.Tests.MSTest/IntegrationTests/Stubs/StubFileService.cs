using System.Reflection;
using FantasticalLog.Core.Contracts;

namespace FantasticalLog.Core.Tests.MSTest.IntegrationTests.Stubs;
public class StubFileService: IFileService
{
    public string ExecutingFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? @".\";
    public string ResourcesTestFolder => Path.Combine(ExecutingFolder, "Resources");
    public string TemporaryTestFolder => Path.Combine(ExecutingFolder, "TempDir");
    public string TemporaryFolderPath => Path.Combine(TemporaryTestFolder, "FantasticalLogTempDir");

    public async Task CreateTemporaryFolder()
    {
        await Task.Run(() =>
        {
            CreateFolder(TemporaryFolderPath);
        });
    }

    public void CreateFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
        Directory.CreateDirectory(folderPath);
    }
}
