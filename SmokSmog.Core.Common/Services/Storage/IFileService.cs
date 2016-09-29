using System;
using System.Threading.Tasks;

namespace SmokSmog.Services.Storage
{
    public interface IFileService
    {
        /// <summary>
        /// Save text file to storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <param name="contents">text content</param>
        void SaveTextFile(string filename, string contents);

        /// <summary>
        /// Load text file from storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <returns>text content</returns>
        String LoadTextFile(string filename);

        /// <summary>
        /// Save asynchronously text file to storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <param name="contents">text content</param>
        /// <returns></returns>
        Task SaveTextFileAsync(string filename, string contents);

        /// <summary>
        /// Load asynchronously text file from storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <returns>text content</returns>
        Task<String> LoadTextFileAsync(string filename);

        /// <summary>
        /// Save binary file to storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <param name="contents">binary content</param>
        void SaveBinaryFile(string filename, byte[] contents);

        /// <summary>
        /// Load binary file from storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <returns>binary content</returns>
        byte[] LoadBinaryFile(string filename);

        /// <summary>
        /// Save asynchronously binary file to storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <param name="contents">binary content</param>
        /// <returns></returns>
        Task SaveBinaryFileAsync(string filename, byte[] contents);

        /// <summary>
        /// Load asynchronously binary file from storage
        /// </summary>
        /// <param name="filename">ex: /mydir/myfile.txt</param>
        /// <returns>binary content</returns>
        Task<byte[]> LoadBinaryFileAsync(string filename);
    }
}