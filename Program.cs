using MultiThreading;
using System.Security;

string mountedPath = @"/log";
string fileName = @"out.txt";

Console.WriteLine("Program Starting!");

var fileHandler = FileHandler.GetInstance(mountedPath, fileName);
var threadHandler = new ThreadHandlerWithExplicitThread(fileHandler);

try
{
    var isCompleted = threadHandler.GenerateFileUpdateThreads();
    if (isCompleted)
    {
        Console.WriteLine("Succesfully generated the file content:");
        var fileContent = fileHandler.GetContent();
        Console.WriteLine(fileContent);
    }
}
catch (IOException ioException)
{
    Console.WriteLine($"IO Errors:  {ioException.Message}");
}
catch (SecurityException securityException)
{
    Console.WriteLine($"File Access Issues:  {securityException.Message}");
}
catch (AggregateException aggregateException)
{
    Console.WriteLine($"Application Execution failied due to error(s):  {aggregateException.Message}");
}

Console.WriteLine("Program Ended, Press any key to exit.");

_ = Console.Read();