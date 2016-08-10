using System.IO;

namespace Easemob.Restfull4Net.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// 说明：用完需关闭
        /// </summary>
        /// <param name="filePath">文件物理路径</param>
        public static FileStream GetFileStream(string filePath)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                fileStream = new FileStream(filePath, FileMode.Open);
            }
            return fileStream;
        }

        /// <summary>
        /// 得到文件信息
        /// </summary>
        /// <param name="filePath">相对路径</param>
        public static FileInfo GetFileInfo(string filePath)
        {
            return new FileInfo(filePath);
        }
    }
}
