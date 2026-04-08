using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace GitConsole
{
    /// <summary>
    /// 封装 git 命令执行，返回 stdout / stderr 及退出码。
    /// </summary>
    public static class GitRunner
    {
        public struct Result
        {
            public string Output;
            public string Error;
            public int ExitCode;
            public bool Success => ExitCode == 0;
        }

        /// <summary>运行 git 命令，工作目录为项目根目录（Assets 的上一级）。</summary>
        public static Result Run(string arguments)
        {
            string workDir = System.IO.Path.GetDirectoryName(Application.dataPath);

            var psi = new ProcessStartInfo
            {
                FileName               = "git",
                Arguments              = arguments,
                WorkingDirectory       = workDir,
                UseShellExecute        = false,
                CreateNoWindow         = true,
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding  = Encoding.UTF8,
            };

            using (var process = new Process { StartInfo = psi })
            {
                var stdout = new StringBuilder();
                var stderr = new StringBuilder();

                process.OutputDataReceived += (_, e) =>
                {
                    if (e.Data != null) stdout.AppendLine(e.Data);
                };
                process.ErrorDataReceived += (_, e) =>
                {
                    if (e.Data != null) stderr.AppendLine(e.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                bool exited = process.WaitForExit(30_000); // 最多等 30 秒
                if (!exited)
                {
                    try { process.Kill(); } catch { /* ignore */ }
                    return new Result
                    {
                        Output   = stdout.ToString().TrimEnd(),
                        Error    = "git command timed out after 30 seconds.",
                        ExitCode = -1,
                    };
                }

                // 等待异步流读取完毕（WaitForExit(timeout) 不保证流已排空）
                process.WaitForExit();

                return new Result
                {
                    Output   = stdout.ToString().TrimEnd(),
                    Error    = stderr.ToString().TrimEnd(),
                    ExitCode = process.ExitCode,
                };
            }
        }
    }
}
