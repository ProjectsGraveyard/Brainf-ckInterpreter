# Brainf-ck Interpreter

An interpreter for the Brainf-ck programming language

Read more about Brainf-ck on
[Wikipedia](https://en.wikipedia.org/wiki/Brainfuck).

# Brainf-ck Interpreter

## How to use

* `Brainf-ckInterpreter.exe /f full_path_to_file_to_interprete.bf`
Run a script from file, the full path needs to be supplied.

* `Brainf-ckInterpreter.exe /i one_line_brainf-ck_commands`
(`/i` can be anything other than `/f`) Run Brainf-ck commands contained in the
2nd argument.

* `Brainf-ckInterpreter.exe /debug`
(`/debug` can be anything but only one argument can be supplied) Run a
hard-coded script, this is for debugging purposes only.

* `Brainf-ckInterpreter.exe`
(without argument) Show usage.

## Standards

This interpreter follows the standards on Wikipedia.

Hardware limitations are not checked, when they are reached, this interpreter
will have undefined behaviour (probably crash).

Specific behaviours on controversial issues:
* Brainf-ck script size: up to 2,147,483,647 bytes
* Cell size: 64 bit integer
* Array pointer: -2,147,483,648 to 2,147,483,647
* End of file: No change (Press F1 for end of file)
* Both cell value and array pointer will overflow and underflow, it will behave
  the same way as C# (No checks in the interpreter, relies on the behaviour of
  C#).
* Array pointer overflowing and underflowing will cause undefined behaviour if
  the other end is not initialized. In real practice, the user probably will
  run out of DRAM first (reaching an end will use more than 120 GB of DRAM).

**Planned features:** Debug breakpoints, memory dump, input from file,
configurable behaviours

# Brianf-ck GUI

Allows the user to easily run multi-line script, a temporary file `temp.bf`
will be generated in the same directory as the GUI application in order to
launch the Interpreter. The Interpreter must be in the same directory as the
GUI application.

**Planned features:** Open file, save file, graphical debugger
