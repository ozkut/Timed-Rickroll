using System;
using System.Diagnostics;
using System.Timers;

namespace TimedRickroll
{
    internal class Program
    {
        private static int cursorPosLeft = 0;

        static void Main(string[] args)
        {
            Console.CursorVisible = true;

            Console.Write("Enter custom link (leave blank for Rickroll): ");//46
            string link = Console.ReadLine(); 
            if (link == string.Empty) link = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley";
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

            //Console.CursorVisible = false;
            
            DisplayTime(false);

            Console.CursorTop = 2;
            Console.Write("Enter time of Rickroll: ");

            Timer DisplayTime_Timer = new(1000) { AutoReset = true };
            DisplayTime_Timer.Elapsed += (source, e) => DisplayTime(false);
            DisplayTime_Timer.Start();

            TimeSpan enteredTime = TimeSpan.MinValue;
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
            DisplayTime_Timer.Elapsed += (source, e) => DisplayTime(true);
            DisplayTime_Timer.Start();

            Timer DisplayCountdown_Timer = new(1000) { AutoReset = true };
            DisplayCountdown_Timer.Elapsed += (source, e) => DisplayCountdown(enteredTime);

            DisplayTime_Timer.Stop();
            DisplayCountdown_Timer.Start();
            System.Threading.Thread.Sleep(20);
            DisplayTime_Timer.Start();

            //Console.WriteLine(timeDifference.TotalSeconds);

            Timer timer = new((enteredTime - DateTime.Now.TimeOfDay).TotalMilliseconds) { AutoReset = false };
            timer.Elapsed += (source, e) => TimerElapsed(link);
            timer.Start();

            //Console.ForegroundColor = ConsoleColor.DarkRed;
            //Console.Clear();
            //Console.WriteLine("Entered time has already passed!");

            Console.ReadKey();
            Environment.Exit(Environment.ExitCode);
        }

        private static void TimerElapsed(string link)
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
        }

        private static void DisplayCountdown(TimeSpan enteredTime)
        {
            Console.SetCursorPosition(0,3);
            Console.Write($"Remaining time: {enteredTime - DateTime.Now.TimeOfDay + TimeSpan.FromSeconds(1):hh\\:mm\\:ss}");
        }

        private static void DisplayTime(bool timeEntered)
        {
            cursorPosLeft = Console.CursorLeft - 24;
            Console.SetCursorPosition(0,1);
            Console.Write($"Current time: {DateTime.Now:HH:mm:ss} (hours:minutes:seconds)");
            if (!timeEntered) Console.SetCursorPosition(cursorPosLeft + 24, 2);
        }
    }
}