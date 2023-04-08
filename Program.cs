using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TimedRickroll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new(DisplayTime);
            thread.Start();

            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan enteredTime))
                return;

            Console.WriteLine("Starting countdown...");
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
                cursorPosLeft = Console.CursorLeft - Text.Length;
            }
        }
    }
}