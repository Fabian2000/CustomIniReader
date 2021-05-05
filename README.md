# CustomIniReader
Custom Ini File Reader written in C# only

## What is CustomIniReader?
CustomIniReader is as the name is already called an ini file reader / editor. It contains many great functions to edit ini files. This script can read, write, delete and check if something exists. This script is completely written in C# only

## Why / When should I use this script?
Editing ini files is not difficult in C# and there are already many scripts / codes which exist to make the edit easier. The most codes are not made in C# only. Many of them containing GetPrivateProfileString and GetPrivateProfileString from C++ imported. The question is, if you should use this, when it is also possible in just C#. This is what this script can do and it also gives you much more possibilities for editing the ini files

### Requirements
CustomIniReader is written in C# .Net Framework 4.8, but it also supports other versions of C# .Net

## How can I use CustomIniReader?
CustomIniReader has its own class. Always when you initialize the class you need to give it a ini filename, otherwise it doesn't work.
Let's say we initialize the class CustomIniReader and call it "test". In that case we can use the methods with "test". For the class we always need to give an argument (string), so the script will know, which file you want to use.
The CustomIniReader methods always have a return value. It could be string, bool, int and float, but it's never empty like a void. In that case you are also able to use for example `Console.WriteLine()` to just check, if it worked or not.

### Example:
```cs
CustomIniReader test = new CustomIniReader("test.ini"); // Initialize the class and add the ini file
Console.WriteLine(test.ReadString("Test", "Example"));
```

## All Codes
-READ- The read method supports the return values as string, bool, int and float
```cs
string ReadString(string key, string section = null);
bool ReadBool(string key, string section = null);
int ReadInt(string key, string section = null);
float ReadFloat(string key, string section = null);
```

-Exists- The exist method can check, if a key value or a section in an ini file exist and returns a bool
```cs
bool KeyExists(string key, string section = null);
bool SectionExists(string section);
```

-Write- The write method supports the return value as bool only, but the arguments may be as string, bool, int and float
```cs
bool WriteString(string content, string key, string section = null);
bool WriteBool(bool content, string key, string section = null);
bool WriteInt(int content, string key, string section = null);
bool WriteFloat(float content, string key, string section = null);
```

-Delete- Delete can delete a single key, but also a whole section
```cs
bool DeleteKey(string key, string section = null);
bool DeleteSection(string section);
```

I tried my best with developing this, but if you find any mistake or you have any kind of problem or question, then feel free to message me on Discord -> Fabian#3563 (Don't message me for fun!)
