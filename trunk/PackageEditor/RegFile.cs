/*
 * Purpose: Reading *.reg files
 * 
 * Currently supports most reg files and types used, with the limitation that 
 * they need to use the default regedit formatting.. 
 * 
 * No 'application specific' functions will be implemented in this class.
 * 
 * Usage:
RegFileRegistryKey regFileRegistryKey;
while (regFile.readKey(out regFileRegistryKey))
{
  foreach (RegFileRegistryValue regFileRegistryValue in regFileRegistryKey.values)
  {
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace PackageEditor.RegFileReader
{
  public class RegFileRegistryValue
  {
    public RegistryValueKind type;
    public String name;
    public object value;
  }

  public class RegFileRegistryKey
  {
    public String keyName;
    public List<RegFileRegistryValue> values = new List<RegFileRegistryValue>();
  }

  class RegFile
  {
    StreamReader sr;
    String NextMasterKey = null;
    Dictionary<string, RegistryValueKind> regFileRegType = new Dictionary<string, RegistryValueKind>();
    public String errors = "";

    public RegFile(String fileName)
    {
      errors = "";
      FileStream fs = new FileStream(fileName, FileMode.Open);
      BufferedStream bs = new BufferedStream(fs);
      sr = new StreamReader(bs);

      regFileRegType.Add("\"", RegistryValueKind.String);
      regFileRegType.Add("hex(2):", RegistryValueKind.ExpandString);
      regFileRegType.Add("hex(7):", RegistryValueKind.MultiString);
      regFileRegType.Add("hex:", RegistryValueKind.Binary);
      regFileRegType.Add("dword:", RegistryValueKind.DWord);
      regFileRegType.Add("hex(b):", RegistryValueKind.QWord);
    }
    public void Close()
    {
      if (sr != null)
      {
        sr.Close();
        sr = null;
      }
    }
    ~RegFile(){
      Close();
    }
    
    public bool readKey(out RegFileRegistryKey myRegistryKey)
    {
      myRegistryKey = new RegFileRegistryKey();      
      while (!sr.EndOfStream)
      {
        String line = sr.ReadLine();
        if (String.IsNullOrEmpty(line))
          continue;
        if (line[0] == '[')
        {
          String keyName = line.Trim('[', ']');
          if (NextMasterKey != null)
          {
            myRegistryKey.keyName = NextMasterKey;
            NextMasterKey = keyName;
            return true;
          }
          else
          {
            NextMasterKey = keyName;
          }
        }
        else
        if (line[0] == '"' || line[0] == '@')
        {
          RegFileRegistryValue regValue = new RegFileRegistryValue();

          bool more = line.EndsWith("\\");
          if (more)
          {
            String extra;
            line = line.Remove(line.Length - 1);
            while (more)
            {
              extra = sr.ReadLine();
              more = extra.EndsWith("\\");
              if (more)
                extra = extra.Remove(extra.Length - 1);
              line += extra.Trim();
            }
          }
          int i = 0;
          bool isInString = false;

          String valueName = "";
          while (isInString || line[i] != '=')
          {
            if (line[i] == '\"')
              isInString = !isInString;
            else
            {
            if (line[i] == '\\')
              i++;
              valueName += line[i];
            };
            i++;
          }
          if (valueName == "@" && i == 1)
            valueName = "";
          String valueValue = line.Substring(i + 1);
          regValue.name = valueName;
          regValue.type = RegistryValueKind.Unknown;
          foreach(KeyValuePair<string, RegistryValueKind> value in regFileRegType)
          {
            if (valueValue.StartsWith(value.Key))
            {
              regValue.type = value.Value;
              break;
            }
          }

          switch (regValue.type)
          {
            case RegistryValueKind.String:
              regValue.value = valueValue.Substring(1, valueValue.Length - 2);
              break;
            case RegistryValueKind.DWord:
              regValue.value = int.Parse(valueValue.Substring(6), System.Globalization.NumberStyles.HexNumber);
              break;
            case RegistryValueKind.Binary:
              {
                String strdata = valueValue.Substring(4);
                byte[] byteArray = RegFileHelper.HexCommaToByteArray(strdata);
                regValue.value = byteArray;
                break;
              }
            case RegistryValueKind.QWord:
              {
                String strdata = valueValue.Substring(7);
                byte[] byteArray = RegFileHelper.HexCommaToByteArray(strdata);
                long l = BitConverter.ToInt64(byteArray, 0);
                regValue.value = l;
                break;
              }
            case RegistryValueKind.ExpandString:
              {
                String strdata = valueValue.Substring(7);
                byte[] byteArray = RegFileHelper.HexCommaToByteArray(strdata);
                String regString = Encoding.Unicode.GetString(byteArray);
                regString = regString.Remove(regString.Length - 1);
                regValue.value = regString;
                break;
              }
            case RegistryValueKind.MultiString:
              {
                String strdata = valueValue.Substring(7);
                byte[] byteArray = RegFileHelper.HexCommaToByteArray(strdata);
                String regString = Encoding.Unicode.GetString(byteArray);
                regString = regString.Remove(regString.Length - 2);
                String[] value = regString.Split(new char[] { '\0' });
                regValue.value = value;
                break;
              }
            default:
              errors += String.Format("\r\n------------------------------------------------\r\nRegistry value not supported Valuename:{0}\r\nType:{1}\r\nValue:{2}\r\nIn key:{3}", regValue.name, regValue.type, valueValue, NextMasterKey);
              break;
          }
          myRegistryKey.values.Add(regValue);            
        }
      }
      if (NextMasterKey != null)
      {
        myRegistryKey.keyName = NextMasterKey;
        NextMasterKey = null;
        return true;
      }
      myRegistryKey = null;
      return false;
    }


    static public void ByteArrayFormatString(ref string byteArray, int firstPrefixLen)
    {
      String result = byteArray;
      int baseNum = firstPrefixLen;
      int count = (result.Length);
      int i = (79 - firstPrefixLen) / 3 * 3;
      i = Math.Max(i, 3);
      while (i < count)
      {
        result = result.Insert(i, "\\\r\n  ");
        count += 5;
        i += 80;
      }
      byteArray = result;
    }
  }

  class RegFileHelper
  {
    public static string ByteArrayToHexComma(byte[] byteArray)
    {
      // input : byte[] 0x4041424344
      // output: string "40,41,42,43,44"

      String result = "";
      for (int i = 0; i < byteArray.Length; i++)
      {
        result += string.Format("{0:x2},", byteArray[i]);
      }
      if (byteArray.Length > 0)
        result = result.Remove(result.Length - 1);
      return result;
    }

    public static byte[] HexCommaToByteArray(string strdata)
    {
      // input : string "40,41,42,43,44"
      // output: byte[] 0x4041424344
      byte[] result = new byte[(strdata.Length + 1) / 3];
      for (int i2 = 0; i2 < result.Length; i2++)
      {
        result[i2] = Convert.ToByte(strdata.Substring(i2 * 3, 2), 16);
      }
      return result;
    }
  }
}
