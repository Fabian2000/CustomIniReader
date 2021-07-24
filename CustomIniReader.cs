/*
MIT License

Copyright (c) 2021 Fabian2000

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Globalization;

namespace CustomIniReaderLib
{
    class CustomIniReader
    {
        /* Variables */
        private string _FilePath = null;

        /* Methods */
        public CustomIniReader(string filepath)
        {
            _FilePath = filepath;
        }

        private bool _IsIniComment(string data)
        {
            if (data.Length <= 0)
            {
                return false;
            }

            if(data[0] == ';')
            {
                return true;
            }

            return false;
        }

        private bool _IsIniSection(string data)
        {
            if (data.Length <= 0)
            {
                return false;
            }

            if (data[0] == '[' && data[data.Length - 1] == ']')
            {
                return true;
            }

            return false;
        }

        public string ReadString(string key, string section = null)
        {
            if (!File.Exists(_FilePath))
            {
                return null;
            }

            string LastSection = null;
            foreach (string line in File.ReadLines(_FilePath))
            {
                if (_IsIniComment(line))
                {
                    continue;
                }

                if (_IsIniSection(line))
                {
                    LastSection = line.Substring(1, line.Length - 2);
                    continue;
                }

                if (line.Contains("=") && LastSection == section)
                {
                    string GetKey = line.Split('=')[0];

                    if (GetKey == key)
                    {
                        return line.Substring(GetKey.Length + 1, line.Length - GetKey.Length - 1);
                    }
                }
            }

            return null;
        }

        public bool ReadBool(string key, string section = null)
        {
            string IniString = ReadString(key, section);

            if(IniString.ToLower() == "true" || IniString == "1")
            {
                return true;
            }

            return false;
        }

        public int ReadInt(string key, string section = null)
        {
            string IniString = ReadString(key, section);

            try
            {
                return Convert.ToInt32(IniString);
            }
            catch {}

            return 0;
        }

        public float ReadFloat(string key, string section = null)
        {
            string IniString = ReadString(key, section);

            try
            {
                return float.Parse(IniString, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch {}

            return 0;
        }

        public bool KeyExists(string key, string section = null)
        {
            if (!File.Exists(_FilePath))
            {
                return false;
            }

            string LastSection = null;
            foreach (string line in File.ReadLines(_FilePath))
            {
                if (_IsIniComment(line))
                {
                    continue;
                }

                if (_IsIniSection(line))
                {
                    LastSection = line.Substring(1, line.Length - 2);
                    continue;
                }

                if (line.Contains("=") && LastSection == section)
                {
                    string GetKey = line.Split('=')[0];

                    if (GetKey == key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool SectionExists(string section)
        {
            if (!File.Exists(_FilePath))
            {
                return false;
            }

            foreach (string line in File.ReadLines(_FilePath))
            {
                if (_IsIniComment(line))
                {
                    continue;
                }

                if (_IsIniSection(line))
                {
                    if (section == line.Substring(1, line.Length - 2))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool WriteString(string content, string key, string section = null)
        {
            if (!File.Exists(_FilePath))
            {
                File.WriteAllText(_FilePath, null);
            }

            /* Append new section */
            if(section != null)
            {
                if(!SectionExists(section))
                {
                    string IniFile = File.ReadAllText(_FilePath);
                    IniFile += "\n[" + section + "]\n" + key + "=" + content;
                    File.WriteAllText(_FilePath, IniFile);

                    return true;
                }
            }

            /* Overwrite existing value */
            if(KeyExists(key, section))
            {
                string LastSection = null;
                List<string> DocumentLines = new List<string>();

                foreach (string line in File.ReadLines(_FilePath))
                {
                    string EditableLine = line;
                    if (_IsIniComment(EditableLine))
                    {
                        DocumentLines.Add(EditableLine);
                        continue;
                    }

                    if (_IsIniSection(EditableLine))
                    {
                        LastSection = EditableLine.Substring(1, EditableLine.Length - 2);
                        DocumentLines.Add(EditableLine);
                        continue;
                    }

                    if (EditableLine.Contains("=") && LastSection == section)
                    {
                        string GetKey = EditableLine.Split('=')[0];

                        if (GetKey == key)
                        {
                            EditableLine = key + "=" + content;
                        }
                    }

                    DocumentLines.Add(EditableLine);
                }

                File.WriteAllLines(_FilePath, DocumentLines);

                return true;
            }

            /* Add Item */
            if(!KeyExists(key, section))
            {
                string LastSection = null;
                List<string> DocumentLines = new List<string>();
                bool Written = false;

                foreach (string line in File.ReadLines(_FilePath))
                {
                    string EditableLine = line;
                    if (_IsIniComment(EditableLine))
                    {
                        DocumentLines.Add(EditableLine);
                        continue;
                    }

                    if (EditableLine == "")
                    {
                        if(LastSection == section)
                        {
                            DocumentLines.Add(key + "=" + content);
                            Written = true;
                        }
                    }

                    if (_IsIniSection(EditableLine))
                    {
                        if(LastSection == section && !Written)
                        {
                            DocumentLines.Add(key + "=" + content);
                        }

                        LastSection = EditableLine.Substring(1, EditableLine.Length - 2);
                        DocumentLines.Add(EditableLine);
                        continue;
                    }

                    DocumentLines.Add(EditableLine);
                }

                File.WriteAllLines(_FilePath, DocumentLines);

                return true;
            }

            return false;
        }

        public bool WriteBool(bool content, string key, string section = null)
        {
            return WriteString(content.ToString(), key, section);
        }

        public bool WriteInt(int content, string key, string section = null)
        {
            return WriteString(content.ToString(), key, section);
        }

        public bool WriteFloat(float content, string key, string section = null)
        {
            return WriteString(content.ToString().Replace(",", "."), key, section);
        }

        public bool DeleteKey(string key, string section = null)
        {
            if (!File.Exists(_FilePath))
            {
                return false;
            }

            List<string> DocumentLines = new List<string>();
            string LastSection = null;
            foreach (string line in File.ReadLines(_FilePath))
            {
                if (_IsIniComment(line))
                {
                    DocumentLines.Add(line);
                    continue;
                }

                if (_IsIniSection(line))
                {
                    LastSection = line.Substring(1, line.Length - 2);
                    DocumentLines.Add(line);
                    continue;
                }

                if (line.Contains("=") && LastSection == section)
                {
                    string GetKey = line.Split('=')[0];

                    if (GetKey == key)
                    {
                        continue;
                    }
                }

                DocumentLines.Add(line);
            }

            File.WriteAllLines(_FilePath, DocumentLines);

            return true;
        }

        public bool DeleteSection(string section)
        {
            if (!File.Exists(_FilePath))
            {
                return false;
            }

            List<string> DocumentLines = new List<string>();
            string LastSection = null;
            bool AddToDocument = true;
            foreach (string line in File.ReadLines(_FilePath))
            {
                if (_IsIniComment(line))
                {
                    DocumentLines.Add(line);
                    continue;
                }

                if (_IsIniSection(line))
                {
                    LastSection = line.Substring(1, line.Length - 2);
                    AddToDocument = true;

                    if (LastSection == section)
                    {
                        AddToDocument = false;
                    }

                    if (AddToDocument)
                    {
                        DocumentLines.Add(line);
                    }

                    continue;
                }

                if (AddToDocument)
                {
                    DocumentLines.Add(line);
                }
            }

            File.WriteAllLines(_FilePath, DocumentLines);

            if (!SectionExists(section))
            {
                return true;
            }

            return false;
        }
    }
}
