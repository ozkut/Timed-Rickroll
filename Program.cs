using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TimedRickroll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread thread0 = new(DisplayTime);
            thread0.Start();

            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan enteredTime))
                return;
            
            Thread thread1 = new(new ThreadStart(delegate () { DisplayCountdown(enteredTime); }));
            thread1.Start();

            while (true)
            {
                if (DateTime.Now.TimeOfDay == enteredTime)
                {
                    Process process = new() 
                    { 
                        StartInfo = new()
                        {
                            UseShellExecute = true,
                            FileName = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley"
                        } 
                    };
                    process.Start();
                    break;
                }
            }
        }

        private async static void DisplayCountdown(TimeSpan enteredTime)
        {
            const string Text = "Remaining time: ";
            Console.Write(Text);
            while (true) 
            {
                TimeSpan remainingTime = enteredTime - DateTime.Now.TimeOfDay;
                Console.SetCursorPosition(Text.Length,2);
                Console.Write(remainingTime.Seconds);
                await Task.Delay(1000);
                Console.SetCursorPosition(Text.Length,2);
                Console.Write(new string(' ', Console.BufferWidth));
            }
        }

        private async static void DisplayTime()
        {
            Console.CursorVisible = false;
            Console.CursorTop = 1;

            int cursorPosLeft = 0;
            const string Text = "Enter time of Rickroll: ";
            Console.Write(Text);

            while (true)
            {
                Console.SetCursorPosition(0,0);
                Console.Write($"Current time: {DateTime.Now:HH:mm:ss} (hour:minute:second)");
                Console.SetCursorPosition(Text.Length + cursorPosLeft, 1);
                await Task.Delay(1000);
                Console.CursorTop -= 1;
                cursorPosLeft = Console.CursorLeft - Text.Length;
            }
        }
    }
}