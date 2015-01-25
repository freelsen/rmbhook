#RmbHook
This project contains two parts. One is a C# mouse and keyboard global library. One is an application based on it.

1> MouseKeyboardLibrary class library;
-The library was originally found here:
http://www.codeproject.com/Articles/28064/Global-Mouse-and-Keyboard-Library;
-And I haven't made changes to it;

2> RmbHook application;
-I want to realize some functions based on the hook lib;
-Now the first, which is also the most important one, is the RmbKey hook function.
-How it works: 
, By tracking the Escape key, it will have a command mode, just like the Vim editor. 
, In the command mode, the keyboard inputs will be processed as commands, to realize functions like cursor moving and so on.
, In this way, when you type words, you need not have to move the cursor by mouse, your hands need not leave the keyboard.
-In contract with Vim, this is a golbal hook, so it works in all situation when you're typing something, not just the Vim editor.

3> More functions to work on;
In the future, these functions are under plan( I had tested yet)
-mouse gesture recognize;
-desktop windows management;
-do some convinent jobs trigged by key command, for example: open a editor for typing some notes.



