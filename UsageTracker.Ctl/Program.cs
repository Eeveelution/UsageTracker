using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

if(!File.Exists("processes"))
    File.WriteAllText("processes", "");

ConsoleHelper.DrawProcessInformation(processes);

ReadCommand:

Console.WriteLine("\nAvailabnle Commands:");
Console.WriteLine("add // remove // proclist // refresh");

string command = Console.ReadLine();

switch (command) {
    case "add": {
        Console.Write("Process Name: ");
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

        List<string> processFile = File.ReadAllLines("processes").ToList();
        processFile.Add(processName);

        File.WriteAllLines("processes", processFile);

        break;
    }
    case "remove": {
        Console.Write("Process Name: ");
        string processName = Console.ReadLine();

        File.Delete($"processInfo/{processName}");

        List<string> processFile = File.ReadAllLines("processes").ToList();
        processFile.Remove(processName);

        File.WriteAllLines("processes", processFile);

        break;
    }
    case "proclist": {
        List<Process> processList = Process.GetProcesses().ToList();

        Console.WriteLine();

        int i = 0;

        foreach (Process process in processList.GroupBy(p => p.ProcessName).Select(p => p.First())) {
            if(i % 4 == 0)
                Console.Write("\n");

            Console.Write($" {process.ProcessName} |");
            i++;
        }

        break;
    }


    case "refresh": {
        processes.Clear();

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
        break;
    }

}

goto ReadCommand;