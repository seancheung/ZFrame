using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class PlayerPrefx
{
    public static void SetValue<T>(string key, T value)
    {
        switch (Type.GetTypeCode(typeof (T)))
        {
                //case TypeCode.Empty:
                //    break;
            case TypeCode.Object:
            {
                if (value == null)
                {
                    Debug.LogError("Cannot set null value of type " + typeof (T));
                    break;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, value);
                    string data = Convert.ToBase64String(ms.ToArray());
                    SetValue(key, data);
                }
            }
                break;
                //case TypeCode.DBNull:
                //    break;
            case TypeCode.Boolean:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.Int32:
                PlayerPrefs.SetInt(key, Convert.ToInt32(value));
                break;
            case TypeCode.UInt64:
            case TypeCode.Int64:
            {
                long val = Convert.ToInt64(value);
                PlayerPrefs.SetInt(key + "_low", (int) (val & uint.MaxValue));
                PlayerPrefs.SetInt(key + "_hight", (int) (val >> 32));
            }
                break;
            case TypeCode.Decimal:
            case TypeCode.Single:
                PlayerPrefs.SetFloat(key, Convert.ToSingle(value));
                break;
            case TypeCode.Double:
                SetValue(key, BitConverter.DoubleToInt64Bits(Convert.ToDouble(value)));
                break;
            case TypeCode.DateTime:
                SetValue(key, Convert.ToDateTime(value).ToBinary());
                break;
            case TypeCode.Char:
            case TypeCode.String:
                PlayerPrefs.SetString(key, Convert.ToString(value));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static T GetValue<T>(string key, T defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;
        return GetValue<T>(key);
    }

    public static T GetValue<T>(string key)
    {
        switch (Type.GetTypeCode(typeof (T)))
        {
            case TypeCode.Empty:
                break;
            case TypeCode.Object:
            {
                string data = GetValue<string>(key);
                if (string.IsNullOrEmpty(data))
                    return default(T);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(data)))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (T) bf.Deserialize(ms);
                }
            }
            case TypeCode.DBNull:
                break;
            case TypeCode.Boolean:
                return (T) (object) Convert.ToBoolean(PlayerPrefs.GetInt(key));
            case TypeCode.Char:
                return (T) (object) Convert.ToChar(PlayerPrefs.GetString(key));
            case TypeCode.SByte:
                return (T) (object) (sbyte) PlayerPrefs.GetInt(key);
            case TypeCode.Byte:
                return (T) (object) (byte) PlayerPrefs.GetInt(key);
            case TypeCode.Int16:
                return (T) (object) (short) PlayerPrefs.GetInt(key);
            case TypeCode.UInt16:
                return (T) (object) (ushort) PlayerPrefs.GetInt(key);
            case TypeCode.Int32:
                return (T) (object) PlayerPrefs.GetInt(key);
            case TypeCode.UInt32:
                return (T) (object) (uint) PlayerPrefs.GetInt(key);
            case TypeCode.Int64:
                return
                    (T)
                        (object)
                            (((long) PlayerPrefs.GetInt(key + "_hight") << 32) | (uint) PlayerPrefs.GetInt(key + "_low"));
            case TypeCode.UInt64:
                return
                    (T)
                        (object)
                            (ulong)
                                (((long) PlayerPrefs.GetInt(key + "_hight") << 32) |
                                 (uint) PlayerPrefs.GetInt(key + "_low"));
            case TypeCode.Single:
                return (T) (object) PlayerPrefs.GetFloat(key);
            case TypeCode.Double:
                return (T) (object) BitConverter.Int64BitsToDouble(GetValue<long>(key));
            case TypeCode.Decimal:
                return (T) (object) (decimal) PlayerPrefs.GetFloat(key);
            case TypeCode.DateTime:
                return (T) (object) DateTime.FromBinary(GetValue<long>(key));
            case TypeCode.String:
                return (T) (object) PlayerPrefs.GetString(key);
            default:
                throw new ArgumentOutOfRangeException();
        }

        throw new ArgumentException(typeof (T) + " is not supported");
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}