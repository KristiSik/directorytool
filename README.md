# DirectoryTool
[![Build status](https://ci.appveyor.com/api/projects/status/au2cjjpwh5gy8lbf?svg=true)](https://ci.appveyor.com/project/KristiSik/directorytool)

**DirectoryTool** is a tool for saving folders into one file, including all files and subfolders.
## Usage
### Saving
`directorytool.exe -s <folder path> <file path>` — this command will save your folder, specified as `<folder path>` parameter into file, which is located in `<file path>` (last will be rewritten, if existed and created, if not).

**Example**
![](https://i.imgur.com/7KTbevk.gif)

### Unpacking
`directorytool.exe -u <file path> <directory path>` — this command will unpack your folder, saved in file, which is specified as `<file path>` parameter, and unpack it into directory, which path is `<directory path>`.

**Example**
![](https://i.imgur.com/Pf02QLC.gif)
