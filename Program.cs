using System;
using System.Diagnostics;
using System.Timers;

namespace TimedRickroll
{
    internal class Program
    {
        private static int cursorPosLeft = 0;
        private static bool timeEntered = false;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = true;

            Console.Write("Enter custom link (leave blank for Rickroll): ");
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

                    System.Threading.Thread.Sleep(1000);

                    Console.SetCursorPosition(46,0);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(46,0);
                    link = Console.ReadLine();
                }
            }

            Console.CursorVisible = false;
            
            DisplayTime();
            Console.CursorTop = 2;
            Console.Write("Enter time of Rickroll: ");

            Timer DisplayTime_Timer = new(1000) { AutoReset = true };
            DisplayTime_Timer.Elapsed += (source, e) => DisplayTime();
            DisplayTime_Timer.Start();

            TimeSpan enteredTime;
            bool timeNotParsed;

            while ((timeNotParsed = !TimeSpan.TryParse(Console.ReadLine(), out enteredTime)) || ((enteredTime - DateTime.Now.TimeOfDay).Seconds <= 0))
            {
                Console.SetCursorPosition(24,2);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(timeNotParsed ? "Incorrect time format!" : "Time is in the past!");
                Console.ResetColor();

                System.Threading.Thread.Sleep(1250);

                Console.SetCursorPosition(24,2);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(24,2);
            }

            DisplayTime_Timer.Stop();
            timeEntered = true;

            Timer DisplayCountdown_Timer = new(1000) { AutoReset = true };
            DisplayCountdown_Timer.Elapsed += (source, e) => 
            {
                Console.SetCursorPosition(0,3);
                Console.Write($"Remaining time: {enteredTime - DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(1):hh\\:mm\\:ss}");
            };
            DisplayCountdown_Timer.Start();

            System.Threading.Thread.Sleep(20);
            DisplayTime_Timer.Start();

            Timer timer = new((enteredTime - DateTime.Now.TimeOfDay).TotalMilliseconds) { AutoReset = false, Enabled = true };
            timer.Elapsed += (source, e) => 
            {
                Process process = new()
                {
                    StartInfo = new()
                    {
                        UseShellExecute = true,
                        FileName = link
                    }
                };
                process.Start();
                DisplayTime_Timer.Stop();
                DisplayCountdown_Timer.Stop();
                Console.Clear();
                Console.WriteLine("Press any key to exit...");
            };

            Console.ReadKey();
            timer.Stop();
            Environment.Exit(Environment.ExitCode);
        }

        private static void DisplayTime()
        {
            cursorPosLeft = Console.CursorLeft - 22;
            Console.SetCursorPosition(0,1);
            Console.Write($"Current time: {DateTime.Now:HH:mm:ss}");
            if (!timeEntered) 
                Console.SetCursorPosition(cursorPosLeft + 22, 2);
        }
    }
}