using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace UsageTracker {
    public class Platform {
        public static void Initialize() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                string systemCtlFile = @$"
[Unit]
Description=Program usage tracker.
Documentation=use the gui, not needed
After=network-online.target
Wants=network-online.target

[Service]
ExecStart=/usr/bin/dotnet {Environment.CurrentDirectory}/UsageTracker.dll
Restart=always

[Install]
WantedBy=default.target";

                File.WriteAllText("systemctl", systemCtlFile);

                File.Copy("systemctl", "~/.config/systemd/user/usagetracker.service", true);

                Process.Start("/bin/bash", "systemctl --user enable usagetracker");
                Process.Start("/bin/bash", "systemctl --user start usagetracker");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                //?????????????????????
            }
        }
    }
}
