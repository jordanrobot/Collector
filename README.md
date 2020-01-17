# iLogicCollector

An Inventor iLogic tool that compiles multiple Visual Basic files into a single file.
This file can be run from inside Inventor's iLogic environment.  This is most useful
when you want to code in an external IDE such as Visual Studio, or when working on an
add-in that you want to perform quick testing on during development. The script will
collect any source code files within a single directory that contains the proper tag
in the first line and insert them into a single iLogic file.  This is most often used
when coding Inventor iLogic routines in Visual Studio.

## Use

To use, include tags into the source code files that you want to collect.

* ``</iLogicCollector>`` : include this tag in the first line of each file you want to collect.
* ``</iLogicCollectorHeader>`` : Include this tag in the first line of header and main files.  This will ensure the file is placed at the top of the collected file.
* ``<\IlogicCollectorHide>`` : include this line after any line of code to comment out that line at collection time.  Use this for module wrappers that help VS, but need to be removed for iLogic to work properly.

When ready to compile, run this powershell script to combine the files into a single file.
If running from the command line, you may need to issue the command like so if permissions
issues prevent running the powershell script.

``powershell -ExecutionPolicy ByPass -File .\iLogicCollector.ps1``

The current directory name is used as the file name with an .iLogicVB extension.  This script will overwrite any file with that same name.
