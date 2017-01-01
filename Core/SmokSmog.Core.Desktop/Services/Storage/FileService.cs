using System;
using System.Threading.Tasks;

namespace SmokSmog.Services.Storage
{
    public class FileService : IFileService
    {
        public byte[] LoadBinaryFile(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> LoadBinaryFileAsync(string filename)
        {
            throw new NotImplementedException();
        }

        public string LoadTextFile(string filename)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoadTextFileAsync(string filename)
        {
            throw new NotImplementedException();
        }

        public void SaveBinaryFile(string filename, byte[] contents)
        {
            throw new NotImplementedException();
        }

        public Task SaveBinaryFileAsync(string filename, byte[] contents)
        {
            throw new NotImplementedException();
        }

        public void SaveTextFile(string filename, string contents)
        {
            throw new NotImplementedException();
        }

        public Task SaveTextFileAsync(string filename, string contents)
        {
            throw new NotImplementedException();
        }
    }
}