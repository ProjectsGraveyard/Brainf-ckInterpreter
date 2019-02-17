using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace Brainf_ckInterpreter
{
    class Program
    {
        /// <summary>
        /// The name and version of this software
        /// </summary>
        const string about = "Brainf-ck Interpreter v1.0";
        /// <summary>
        /// Instructions
        /// </summary>
        static char[] ins;
        /// <summary>
        /// Instruction pointer
        /// </summary>
        static int iPtr = 0;
        /// <summary>
        /// Positive memory (0 and above)
        /// </summary>
        static List<long> mem = new List<long>();
        /// <summary>
        /// Negative memory (-1 and below)
        /// </summary>
        static List<long> lMem = new List<long>();
        /// <summary>
        /// Memory pointer
        /// </summary>
        static int mPtr = 0;
        /// <summary>
        /// Loop stack, keeps track of where loops go back to
        /// </summary>
        static Stack<int> lStack = new Stack<int>();
        /// <summary>
        /// Keep track of how many comment loops or zero loops are opened
        /// </summary>
        static int clOpened = 0;
        /// <summary>
        /// Temporary variable used in various places
        /// </summary>
        static long temp;

        /// <summary>
        /// Interprete Brainf-ck script
        /// </summary>
        /// <param name="args">
        ///     string array, string at index 1 is expected to be the path to
        ///     file to interprete
        /// </param>
        static void Main(string[] args)
        {
            // Set title
            title();
            // Check argument
            if (args.Length < 2)
            {
                if (args.Length == 1)
                {
                    ins = (",.").ToCharArray();
                }
                else
                {
                    // Show usage (help) if no argument given then EXIT
                    Console.WriteLine("Usage: ");
                    Console.WriteLine(
                        "Brainf-ckInterpreter.exe /f " +
                        "full_path_to_file_to_interprete.bf"
                    );
                    Console.WriteLine(
                        "Brainf-ckInterpreter.exe /i " +
                        "one_line_brainf-ck_commands"
                    );
                    pause();
                    return;
                }
            }
            else if (args[0] == "/f")
            {
                try
                {
                    // Check file size
                    if (new FileInfo(args[1]).Length > Int32.MaxValue)
                    {
                        // File too large, show error message then EXIT
                        title("Error");
                        Console.WriteLine("The file is too large. ");
                        pause();
                        return;
                    }
                    // Convert to instruction array
                    ins = File.ReadAllText(args[1]).ToCharArray();
                }
                catch
                {
                    // Failed to read, show error message then EXIT
                    title("Error");
                    Console.WriteLine("Failed to open " + args[1]);
                    pause();
                    return;
                }
            }
            else
            {
                ins = args[1].ToCharArray();
            }
            // Initialize first memory item
            mem.Add(0);
            // Start interpreting
            title("Running...");
            while (iPtr < ins.Length)
            {
                // Main switch
                switch (ins[iPtr])
                {
                    case '>':
                        // Increate pointer
                        mPtr++;
                        if (mem.Count() == mPtr)
                        {
                            // Increate positive memory size if it is not large
                            // enough
                            mem.Add(0);
                        }
                        break;
                    case '<':
                        // Decreate pointer
                        mPtr--;
                        if (mPtr < lMem.Count() * -1)
                        {
                            // Increate negative memory size if it is not large
                            // enough
                            lMem.Add(0);
                        }
                        break;
                    case '+':
                        // Increate value under pointer
                        setMem(1);
                        break;
                    case '-':
                        // Decreate value under pointer
                        setMem(-1);
                        break;
                    case '.':
                        temp = getMem();
                        if (temp < 0 || temp > 127)
                        {
                            // Cannot convert to ASCII, show error message then
                            // EXIT
                            title("Error");
                            Console.WriteLine();
                            Console.WriteLine(
                                "Error: Cannot convert " + temp.ToString() +
                                " to ASCII. Instruction pointer: " +
                                iPtr.ToString()
                            );
                            pause();
                            return;
                        }
                        else if (temp == 10)
                        {
                            // Handle new line
                            Console.WriteLine();
                        }
                        else
                        {
                            // Print the character
                            Console.Write(Convert.ToChar(temp));
                        }
                        break;
                    case ',':
                        // Let user know an input is expected
                        title("Waiting for user input... F1 for end of file");
                        // Get key
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        // Check for end of file
                        if (key.Key == ConsoleKey.F1)
                        {
                            // No change to current memory
                            break;
                        }
                        // Check input
                        temp = Convert.ToInt64(key.KeyChar);
                        // Check if the input is valid
                        if (temp < 0 || temp > 127)
                        {
                            title("Input not valid");
                            MessageBox.Show(
                                "Your input cannot be converted to a valid " +
                                "ASCII character, please try again. ",
                                about
                            );
                            // Skip to next loop without changing instruction
                            // pointer position
                            continue;
                        }
                        // End of line standard (10)
                        if (temp == 13)
                        {
                            temp = 10;
                        }
                        // Write to memory
                        writeMem(temp);
                        // Debug
                        System.Diagnostics.Debug.WriteLine(temp);
                        break;
                    case '[':
                        if (getMem() == 0)
                        {
                            // Comment loop and zero loop
                            clOpened++;
                            // Save where the loop opened in case it is not
                            // closed
                            temp = iPtr;
                            // Skip to the matching ]
                            while (clOpened != 0)
                            {
                                iPtr++;
                                if (ins.Count() == iPtr)
                                {
                                    // Loop not closed but end of file reached
                                    title("Error");
                                    Console.WriteLine();
                                    Console.WriteLine(
                                        "Error: Comment loop or zero loop " +
                                        "not closed. Loop opened at: " + temp
                                    );
                                    pause();
                                    return;
                                }
                                else if (ins[iPtr] == '[')
                                {
                                    clOpened++;
                                }
                                else if (ins[iPtr] == ']')
                                {
                                    clOpened--;
                                }
                            }
                        }
                        else
                        {
                            // Add loop stack
                            lStack.Push(iPtr);
                        }
                        break;
                    case ']':
                        if (getMem() == 0)
                        {
                            // Exit loop
                            lStack.Pop();
                        }
                        else
                        {
                            // Jump to last loop
                            iPtr = lStack.First();
                        }
                        break;
                    default:
                        // Not an instruction, ignored
                        break;
                }
                // Increate instruction pointer
                iPtr++;
            }
            // End
            if (lStack.Count() == 0)
            {
                title("Program ended");
                Console.WriteLine();
            }
            else
            {
                // Loop not closed but end of file reached
                title("Error");
                Console.WriteLine();
                Console.WriteLine("Error: Loop not closed. Loop stack: ");
                while (lStack.Count() > 0)
                {
                    Console.WriteLine(lStack.Pop());
                }
            }
            pause();
            return;
        }

        /// <summary>
        /// Pause the execution until the user press any key
        /// The key pressed will now be shown on screen
        /// </summary>
        static void pause()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Set the title of the window
        /// </summary>
        /// <param name="t">String to display</param>
        static void title(string t)
        {
            Console.Title = about + " - " + t;
        }
        /// <summary>
        /// Reset the title of the window
        /// </summary>
        static void title()
        {
            Console.Title = about;
        }

        /// <summary>
        /// Get the memory under pointer
        /// </summary>
        /// <returns>The value under pointer</returns>
        static long getMem()
        {
            return (mPtr >= 0) ? mem[mPtr] : lMem[mPtr * -1 - 1];
        }
        /// <summary>
        /// Update a value to the memory under pointer
        /// </summary>
        /// <param name="delta">The amount to change</param>
        static void setMem(int delta)
        {
            if (mPtr >= 0)
            {
                mem[mPtr] += delta;
            }
            else
            {
                lMem[mPtr * -1 - 1] += delta;
            }
        }
        /// <summary>
        /// Set a value to the memory under pointer
        /// </summary>
        /// <param name="val">The value to set</param>
        static void writeMem(long val)
        {
            if (mPtr >= 0)
            {
                mem[mPtr] = val;
            }
            else
            {
                lMem[mPtr * -1 - 1] = val;
            }
        }
    }
}
