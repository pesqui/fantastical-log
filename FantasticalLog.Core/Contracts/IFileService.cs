namespace FantasticalLog.Core.Contracts;
public interface IFileService
{
    string TemporaryFolderPath { get; }
    Task CreateTemporaryFolder();
}
