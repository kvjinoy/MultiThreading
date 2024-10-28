using System.Globalization;

namespace MultiThreading
{
    internal class FileHandler
    {
        private const string TimeFormat = "hh:mm:ss.fff";
        private static FileHandler? _instance;
        private static readonly object _lock = new object();
        private string _filePath;
        private int lineNumber = 0;
        private static Mutex mutex = new Mutex();

        private FileHandler(string folderPath, string filename)
        {
            if (Directory.Exists(folderPath))
            {
                _filePath = Path.Combine(folderPath, filename);
            }
            else
            {
                _filePath = filename;
            }
        }

        public static FileHandler GetInstance(string folderPath, string filename)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new FileHandler(folderPath, filename);
                    }
                }
            }

            return _instance;
        }
    
        public void CreateFile()
        {
            RemoveExistingFile();
            lineNumber = 0;
            string initialContent = $"{lineNumber}, {0}, {DateTime.Now.ToString(TimeFormat)}";
            File.WriteAllText(_filePath, $"{initialContent}\n");
        }

        public void AppendContent(int threadId)
        {
            mutex.WaitOne();
            lineNumber++;
            var content = $"{lineNumber}, {threadId}, {DateTime.Now.ToString(TimeFormat)}";
            AppendContent(new[] { $"{content}" });
            mutex.ReleaseMutex();
        }

        public string GetContent()
        {
            string content = File.ReadAllText(_filePath);
            return content;
        }

        private void AppendContent(string [] contents)
        {
            File.AppendAllLines(_filePath, contents);
        }

        private void RemoveExistingFile()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        ~FileHandler()
        {
            mutex.Dispose();
        }

    }
}
