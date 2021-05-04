# CustomIniReader
Custom Ini File Reader written in C# only

## Ini Reader
Many Reader extensions/scripts for C# are made with GetPrivateProfileString or WritePrivateProfileString, but it is old and possible in just C#.
I made it possible with this little script. I hope you like it: If you find any mistake, feel free to message me on Discord: Fabian#3563 (Don't message me for fun!)

## How to use CustomIniReader?
Example:
```cs
CustomIniReader test = new CustomIniReader("test.ini"); // Initialize the class and add the ini file
Console.WriteLine(test.ReadString("Test", "Example"));
```

## All Codes
Read
```cs
string ReadString(string key, string section = null);
bool ReadBool(string key, string section = null);
int ReadInt(string key, string section = null);
float ReadFloat(string key, string section = null);
```

Exists
```cs
bool KeyExists(string key, string section = null);
bool SectionExists(string section);
```

Write
```cs
bool WriteString(string content, string key, string section = null);
bool WriteBool(bool content, string key, string section = null);
bool WriteInt(int content, string key, string section = null);
bool WriteFloat(float content, string key, string section = null);
```

Delete
```cs
bool DeleteKey(string key, string section = null);
bool DeleteSection(string section);
```

If you have any question or problem: Discord -> Fabian#3563 (Don't message me for fun!)
