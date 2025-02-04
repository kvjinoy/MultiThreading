# MultiThreading Program

Created a C# Windows console application, based on docker Linux container. 
The application will create a file in the location  /log/out.txt" and will Write 0, 0, <current_time_stamp> to the first line of the file, with <current_time_stamp> formatted as HH:mm:SS.fff.

Then the application will Launch 10 threads to run simultaneously.
Each thread appends 10 lines to the file" /log/out.txt", ensuring thread-safe access. 

Each line is written as <line_count>, <thread_id>, <current_time_stamp>, with the line count incrementing sequentially.

Each thread terminates after performing 10 writes.

After all threads have finished, the app will wait for a character press, then exit the application.

**Docker Configuration**

1. Container volume mapping Customization: 
The following syntax included in the Project File to create a volume "C:\junk" and mounts it in the container in the folder /log.
```
<PropertyGroup>
   	<DockerfileRunArguments>-v c:\junk:/log</DockerfileRunArguments>
</PropertyGroup>
```
**Safe File Access**

Created a _FileHandler_ Singleton class to facilitate the file access and operations in a restricted way.
A local Mutex object is used to synchronize the access to the. The calling thread will be blocked until it acquires ownership of the mutex, the ReleaseMutex is used to release ownership of the mutex. A dispose method is included within the FileHandler class to dispose the mutex object.

**Thread Creation**

There are two implementations provided for thread creation.

The class _ThreadHandlerWithExplicitThread_ will create 10 threads explicitly using the _Task.Factory.StartNew_ method with the LongRunning Task Creation Options. 

The class _ThreadHandlerWithImplicitThread_ will create the threads implicitly using the _Task.Run_ method, and the task will be added to the thread pool. This is a more recommended method to follow. 

The aggregated tasks from the class can run asynchronously with synchronous access to the file and append the content within it. The newly created file available under the folder will be /log





