using System;
using System.Diagnostics;

namespace TimedRickroll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = true;

            Console.Write("Enter custom link (leave blank for Rickroll): ");//46
            string link = Console.ReadLine();
            if (link == string.Empty)
                link = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley";
            else
            { 
                while (!(Uri.TryCreate(link, UriKind.Absolute, out Uri result) && result != null && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps)))
                {
                    Console.SetCursorPosition(46,0);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("Link is invalid!");
                    Console.ResetColor();

                    Thread.Sleep(1000);

                    Console.SetCursorPosition(46,0);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(46,0);
                    link = Console.ReadLine();
                }
            }

            Console.CursorVisible = false;

            CancellationTokenSource tokenSource = new();
            ThreadStart threadStart = new(delegate () { DisplayTime(); });

            Thread DisplayTime_Thread = new(threadStart) { IsBackground = true };
            DisplayTime_Thread.Start();

            TimeSpan enteredTime;
            while (!TimeSpan.TryParse(Console.ReadLine(), out enteredTime))
            {
                Console.SetCursorPosition(24,1);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("Incorrect time format!");
                Console.ResetColor();

                Thread.Sleep(1000);

                Console.SetCursorPosition(24, 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(24,1);
            }

            tokenSource.Cancel();
            DisplayTime_Thread = new(threadStart) { IsBackground = true };
            DisplayTime_Thread.Start();

            Thread.Sleep(20);

            Thread DisplayCountdown_Thread = new(new ThreadStart(delegate () { DisplayCountdown(enteredTime); })) { IsBackground = true };
            DisplayCountdown_Thread.Start();

            while (true)
            {
                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                int timeComparison = TimeSpan.Compare(enteredTime, currentTime);

                switch (timeComparison)
                {
                    case 0:
                        Process process = new()
                        {
                            StartInfo = new()
                            {
                                UseShellExecute = true,
                                FileName = link
                            }
                        };
                        process.Start();
                        break;
                    //FIX
                    //case int _ when timeComparison < 0:
                    //    Console.ForegroundColor = ConsoleColor.DarkRed;
                    //    Console.Clear();
                    //    Console.WriteLine("Entered time has already passed!");
                    //    break;
                }
                break;
            }

            Console.ReadKey();
            tokenSource.Dispose();
            Environment.Exit(Environment.ExitCode);
        }

        private async static void DisplayCountdown(TimeSpan enteredTime)
        {
            const string Text = "Remaining time: ";
            Console.Write("\n" + Text);
            while (true) 
            {
                Console.SetCursorPosition(Text.Length,3);
                Console.Write((enteredTime - DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(1)).ToString("hh\\:mm\\:ss"));

                await Task.Delay(1000);

                Console.SetCursorPosition(Text.Length,3);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        private async static void DisplayTime(CancellationToken token = default)
        {
            const string Text = "Enter time of Rickroll: ";
            int cursorPosLeft = 0;

            Console.CursorTop = 2;
            Console.Write(Text);

            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(0,1);
                Console.Write($"Current time: {DateTime.Now:HH:mm:ss} (hours:minutes:seconds)");
                Console.SetCursorPosition(Text.Length + cursorPosLeft, 2);

                try { await Task.Delay(1000, token); } catch (TaskCanceledException) { return; }

                Console.CursorTop = 1;
                cursorPosLeft = Console.CursorLeft - Text.Length;
            }
        }
    }
}