namespace MultiThreading
{
    internal class ThreadHandlerWithImplicitThread : IThreadHandler
    {
        FileHandler _fileHandler;
        private const int numberOfIterations = 10;
        private const int numberOfThreads = 10;

        public ThreadHandlerWithImplicitThread(FileHandler fileHandler) { 

            _fileHandler = fileHandler;     
        }

        public bool GenerateFileUpdateThreads()
        {
            _fileHandler.CreateFile();

            List<Task> tasks = GenerateThreads();
            var taskExecutions = Task.WhenAll(tasks);

            try
            {
                taskExecutions.Wait();
            }
            catch (AggregateException)
            {
                throw;
            }

            return taskExecutions.Status == TaskStatus.RanToCompletion;
        }

        private List<Task> GenerateThreads()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfThreads; i++)
            {
                var task = Task.Run(() =>
                {
                    AppendByThread(Environment.CurrentManagedThreadId);
                });
                tasks.Add(task);
            }

            return tasks;
        }

        private void AppendByThread(int threadId)
        {
            for (int i = 0; i < numberOfIterations; i++)
            {
                 _fileHandler.AppendContent(threadId);
            }
        }

    }
}
