namespace QuickEditor.ToolKit.SVN
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public enum eSVNCmd
    {
        Add,
        Commit,
        Checkout,
        Update,
        Revert,
        Lock,
        Unlock,
        Cleanup,
        Log,
        Settings,
        Help
    }

    public static class SVNControlTools
    {
        private const string SVNProc = "svn";

        //private const string SVNProc = "TortoiseProc.exe";
        private static Dictionary<eSVNCmd, string> kSVNCommands = new Dictionary<eSVNCmd, string>()
        {
            {eSVNCmd.Add, "add" },
            {eSVNCmd.Commit, "commit" },
            {eSVNCmd.Checkout, "checkout" },
            {eSVNCmd.Update, "update" },
            {eSVNCmd.Revert, "revert" },
            {eSVNCmd.Lock, "lock" },
            {eSVNCmd.Unlock, "unlock" },
            {eSVNCmd.Cleanup, "cleanup" },
            {eSVNCmd.Log, "log" },
            {eSVNCmd.Settings, "settings" },
            {eSVNCmd.Help, "help" },
        };

        ///closeonend:0 不自动关闭对话框
        ///closeonend:1 如果没发生错误则自动关闭对话框
        ///closeonend:2 如果没发生错误和冲突则自动关闭对话框
        ///closeonend:3如果没有错误、冲突和合并，会自动关闭
        public static void RunCommand(eSVNCmd eSVNCmd, string path = null, string url = null)
        {
            string command = kSVNCommands[eSVNCmd];
            string argument = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                if (string.IsNullOrEmpty(url))
                {
                    argument = string.Format("/command:{0} /path:{1} /closeonend:0", command, path);
                }
                else
                {
                    argument = string.Format("/command:{0} /url:{1} / path:{2}", command, url, path);
                }
            }
            else
            {
                argument = string.Format("/command:{0}", command);
            }
            ProcessCommand(SVNProc, null);
        }

        private static bool ProcessCommand(string command, string argument)
        {
            EditorUtility.DisplayProgressBar("", command + " " + argument, 1.0f);
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(command);
            info.Arguments = argument;
            info.CreateNoWindow = true;
            info.ErrorDialog = true;
            info.UseShellExecute = false;

            if (info.UseShellExecute)
            {
                info.RedirectStandardOutput = false;
                info.RedirectStandardError = false;
                info.RedirectStandardInput = false;
            }
            else
            {
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.RedirectStandardInput = true;
                info.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
                info.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            }

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

            string log = process.StandardOutput.ReadToEnd();
            bool error = false;
            if (log.Contains("Error"))
            {
                error = true;
                Debug.LogError(log);
            }
            else
            {
                Debug.Log(log);
            }
            process.WaitForExit();
            process.Close();
            EditorUtility.ClearProgressBar();
            return !error;
        }
    }
}