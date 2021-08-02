using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alba.CsConsoleFormat;
using UsageTracker.Common;

namespace UsageTracker.Ctl {
    public class ConsoleHelper {
        private static void OutputDocument(Document document) {
            using (var writer = new StringWriter()) {
                ConsoleRenderer.RenderDocumentToText(document, new TextRenderTarget(writer), new Rect(0, 0, 250, Size.Infinity));

                string str = writer.GetStringBuilder().ToString();

                string[] lines = str.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                    lines[i] = lines[i].TrimEnd();
                str = string.Join('\n', lines);

                Console.Write(str);
            }
        }

        public static void DrawProcessInformation(List<TrackedProcess> processes) {
            OutputDocument(
            new Document(
            new Grid {
                Columns = { GridLength.Auto, GridLength.Auto, GridLength.Auto},
                Children = {
                    new Cell("Application Name"),
                    new Cell("Process Name"),
                    new Cell("Hours Used"),

                    processes.OrderByDescending(p => p.SecondsTotal).Select(item => new[] {
                        new Cell($"{item.Alias}"),
                        new Cell($"{item.ProcessToTrack}"),
                        new Cell($"{Math.Round(((double)item.SecondsTotal / 60.0) / 60.0, 2)}")
                    })
                }
            }
            ));
        }
    }
}
