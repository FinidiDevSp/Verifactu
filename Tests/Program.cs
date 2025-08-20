AppDomain.CurrentDomain.UnhandledException += (s, e) =>
{
    var ex = e.ExceptionObject as Exception;
    File.WriteAllText("fatal-unhandled-exception.txt", ex?.ToString() ?? "(no exception)");
    Console.Error.WriteLine("💥 UnhandledException:\n" + (ex?.ToString() ?? "(null)"));
};

TaskScheduler.UnobservedTaskException += (s, e) =>
{
    e.SetObserved();
    File.WriteAllText("fatal-unobserved-task.txt", e.Exception.ToString());
    Console.Error.WriteLine("💥 UnobservedTaskException:\n" + e.Exception);
};
await QuickSendAlta.Main();