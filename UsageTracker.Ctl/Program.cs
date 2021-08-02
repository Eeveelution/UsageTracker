using System;
using System.Collections.Generic;
using System.IO;
using UsageTracker.Common;
using UsageTracker.Ctl;

List<TrackedProcess> processes = new();

if (Directory.Exists("processInfo")) {
    foreach (string file in Directory.GetFiles("processInfo")) {
        string processName = Path.GetFileName(file);

        TrackedProcess process = TrackedProcess.FromProcessName(processName);

        if (process != null)
            processes.Add(process);
    }
} else {
    Directory.CreateDirectory("processInfo");
}

ConsoleHelper.DrawProcessInformation(processes);

Console.WriteLine("\nAvailabnle Commands:");
Console.WriteLine("add // remove // rename // clear // proclist");

string command = Console.ReadLine();

switch (command) {
    case "add": {
        Console.Write("Process Name:");
        string processName = Console.ReadLine();
        Console.Write("Alias for Process:");
        string alias = Console.ReadLine();

        TrackedProcess process = new() {
            ProcessToTrack = processName,
            Alias          = alias,
            SecondsTotal   = 0,
            SecondsTracked = 0,
            IsRunning      = false
        };

        process.Save();
    }
}

Console.ReadLine();