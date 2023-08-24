using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO.Ports;
using System.IO;
namespace ComDemo
{
    public class Cmn
    {
        public static Byte[] GetMD5(byte[] buffer)
        {
            //创建MD5对象
            MD5 md5 = MD5.Create();
            //开始加密
            //将字符串转换为字节数组
            //byte[] buffer = Encoding.Default.GetBytes(str);
            //返回一个完成加密的字节数组
            byte[] MD5buffer = md5.ComputeHash(buffer);
            ////将字节数组每个元素Tostring()
            //string newstr = "";
            //for (int i = 0; i < MD5buffer.Length; i++)
            //{
            //    //同时10进制转换为16进制
            //    newstr += MD5buffer[i].ToString("x2");
            //}
            //return newstr;
            return MD5buffer;
        }

        public static Byte GetXOR(byte[] buffer, int pos, int len)
        {
            byte ret = 0x00;

            if (len > buffer.Length || len > buffer.Length - pos)
            {
                return ret;
            }

            for (int i = 0; i < len; i++)
            {
                ret ^= buffer[pos + i];
            }

            return ret;
        }

        public static Byte GetXORTboxBl(byte[] buffer, int pos, int len)
        {
            byte ret = 0x00;

            if (len > buffer.Length || len > buffer.Length - pos)
            {
                return ret;
            }

            for (int i = 0; i < len; i++)
            {
                //ret ^= buffer[pos + i];
                ret = (Byte)(ret + buffer[pos + i]);
            }
            ret = (Byte)~ret;

            return ret;
        }

        //类似C语言memset功能
        public static void memset(ref Byte[] data, Byte val, int len)
        {
            for (int i = 0; i < len; i++)
            {
                data[i] = val;
            }
        }

        public static void memset(ref Byte[] data, int pos, Byte val, int len)
        {
            if (pos + len > data.Length)
            {
                return;
            }

            for (int i = 0; i < len; i++)
            {
                data[pos + i] = val;
            }
        }

        public static void memcpy(ref Byte[] dst, int dstPos, Byte[] src, int srcPos, int len)
        {
            if (len > src.Length - srcPos)
            {
                return;
            }

            if (len > dst.Length - dstPos)
            {
                return;
            }

            int j = dstPos;
            for (int i = 0; i < len; i++)
            {
                dst[j] = src[srcPos + i];
                j++; 
            }
        }

        public static bool IsNumber(string numStr)
        {
            if (numStr == null)
            {
                return false;
            }

            bool data16Flag = false;

            try
            {
                numStr = numStr.ToUpper();
                for (int i = 0; i < numStr.Length; i++)
                {
                    if (numStr[i] == 'x' || numStr[i] == 'X')
                    {
                        if (i != 1)
                        {
                            return false;
                        }
                        data16Flag = true;
                    }
                    else
                    {
                        if (data16Flag == false)
                        {
                            if (numStr[i] >= '0' && numStr[i] <= '9')
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if ((numStr[i] >= '0' && numStr[i] <= '9')
                               || (numStr[i] >= 'A' && numStr[i] <= 'Z'))
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string IntToStringFromHex(int Val)
        {
            try
            {
                return "0x" + Convert.ToString(Val, 16);
            }
            catch
            {
                return "";
            }
        }

        public static string IntToString(int Val)
        {
            try
            {
                return Convert.ToString(Val);
            }
            catch
            {
                return "";
            }
        }

        public static string U32ToStrHex(UInt32 Val)
        {
            try
            {
                return "0x" + Val.ToString("X");
            }
            catch
            {
                return "";
            }
        }

        public static string U32ToStr(UInt32 Val)
        {
            try
            {
                return Val.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static int StringToInt(string str)
        {
            try
            {
                if (true == IsNumber(str))
                {
                    if (str.Length <= 2)
                    {
                        return Convert.ToInt32(str);
                    }
                    else
                    {
                        if (str[1] == 'x' || str[1] == 'X')
                        {
                            return Convert.ToInt32(str, 16);
                        }
                        else
                        {
                            return Convert.ToInt32(str);
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static UInt32 StringToUInt32(string str)
        {
            try
            {
                if (true == IsNumber(str))
                {
                    if (str.Length <= 2)
                    {
                        return Convert.ToUInt32(str);
                    }
                    else
                    {
                        if (str[1] == 'x' || str[1] == 'X')
                        {
                            return Convert.ToUInt32(str, 16);
                        }
                        else
                        {
                            return Convert.ToUInt32(str);
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }


        public static string BoolToString(bool Val)
        {
            try
            {
                if (Val == true)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            catch
            {
                return "false";
            }
        }

        public static bool StringToBool(string str)
        {
            try
            {
                if (str == "true" || str == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //字符串转字节流
        public static Byte[] stringToBytes(string data)
        {
            return System.Text.Encoding.Default.GetBytes(data);
        }

        //字节流转字符串
        public static string bytesToString(Byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", " ");
        }

        public static string ByteToString(Byte data)
        {
            return "0x" + data.ToString("x2");
        }

        public static string bytesToString(Byte[] data, int pos, int len)
        {
            if (data.Length <= 0 || pos <= 0 || len <= 0)
            {
                return "";
            }

            Byte[] strBytes = data.Skip(pos).Take(len).ToArray();

            return System.Text.Encoding.Default.GetString(strBytes);
        }

        //字节流转数字--大端
        public static UInt16 BytesToUint16Big(Byte[] data, int pos)
        {
            UInt16 ret = 0;
            if (data.Length - pos < 2)
            {
                return ret;
            }

            ret = System.BitConverter.ToUInt16(data, pos);
            ret = (UInt16)((ret & 0xFFU) << 8 | (ret & 0xFF00U) >> 8);
            return ret;
        }

        public static Byte[] Uint16ToBytesBig(UInt16 val)
        {
            Byte[] bytes = new Byte[2];
            if (val > 0xFFFF)
            {
                return null;
            }
            bytes = System.BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return bytes;
        }

        public static UInt16 BytesToUint16Small(Byte[] data, int pos)
        {
            UInt16 ret = 0;
            if (data.Length - pos < 2)
            {
                return ret;
            }

            ret = System.BitConverter.ToUInt16(data, pos);
            return ret;
        }

        public static Byte[] Uint16ToBytesSmall(UInt16 val)
        {
            Byte[] bytes = new Byte[2];
            if (val > 0xFFFF)
            {
                return null;
            }
            bytes = System.BitConverter.GetBytes(val);
            return bytes;
        }

        //字节流转数字--小端
        public static int BytesToInt32Small(Byte[] data, int pos)
        {
            if (data.Length - pos < 4)
            {
                return 0;
            }

            return System.BitConverter.ToInt32(data, pos);
        }

        public static uint BytesToUint32Small(Byte[] data, int pos)
        {
            if (data.Length - pos < 4)
            {
                return 0;
            }

            return System.BitConverter.ToUInt32(data, pos);
        }

        public static Byte[] Uint32ToBytesSmall(UInt32 val)
        {
            return System.BitConverter.GetBytes(val);
        }

        public static UInt32 BytesToUint32Big(Byte[] data, int pos)
        {
            UInt32 ret = 0;
            if (data.Length - pos < 4)
            {
                return ret;
            }

            ret = System.BitConverter.ToUInt32(data, pos);
            ret = (UInt32)(ret & 0x000000FFU) << 24 | (ret & 0x0000FF00U) << 8 | (ret & 0x00FF0000U) >> 8 | (ret & 0xFF000000U) >> 24; ;
            return ret;
        }

        public static Byte[] Uint32ToBytesBig(UInt32 val)
        {
            Byte[] bytes = new Byte[4];
            if (val > 0xFFFFFFFF)
            {
                return null;
            }
            bytes = System.BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return bytes;
        }

        public static Byte[] GetBytesFromBytes(Byte[] data, int pos, int len)
        {
            return data.Skip(pos).Take(len).ToArray();
        }

        //字节流中查找关键字节流
        public static int IndexOf(Byte[] src, Byte[] dst, int num)
        {
            if (dst.Length > src.Length || dst.Length == 0 || src.Length == 0)
            {
                return -1;
            }

            if (dst.Length == src.Length)
            {
                if (dst != src)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }

            int index = -1;
            int j = 0;
            int cnt = 0;

            //字符小于的情况下判断
            for (int i = 0; i < src.Length; i++)
            {
                if (j != 0)
                {
                    if (src[i] == dst[j])
                    {
                        j++;
                        if (j == dst.Length)
                        {
                            cnt++;
                            if (cnt >= num)
                            {
                                return index;
                            }
                            else
                            {
                                j = 0;
                                index = -1;
                            }
                        }
                    }
                    else
                    {
                        j = 0;
                        index = -1;
                    }
                }
                else
                {
                    if (src[i] == dst[j])
                    {
                        j = 1;
                        index = i;
                    }
                }
            }

            return -1;
        }

        //判断两个数组是否相等
        public static bool CompareArray(byte[] bt1, byte[] bt2)
        {
            var len1 = bt1.Length;
            var len2 = bt2.Length;
            if (len1 != len2)
            {
                return false;
            }

            for (var i = 0; i < len1; i++)
            {
                if (bt1[i] != bt2[i])
                    return false;
            }

            return true;
        }

        public static String fileSaveCurDir(String name, Byte[] fileBytes)
        {
            try
            {
                String path = Environment.CurrentDirectory;
                FileStream fs;
                path = path + "\\" + name;
                //存在则删除文件
                if (false == File.Exists(path))
                {
                    fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
                }
                else
                {
                    fs = new FileStream(path, FileMode.Truncate, FileAccess.ReadWrite);
                }

                if (fs == null)
                {
                    return "文件" + name + "创建失败";
                }
                fs.Write(fileBytes, 0, fileBytes.Length);
                fs.Close();
                return "OK";
            }
            catch
            {
                return "fileSaveCurDir " + name + " err";
            }
        }

        public static String pathSave(String path, Byte[] fileBytes)
        {
            try
            {
                FileStream fs;
                //存在则删除文件
                if (false == File.Exists(path))
                {
                    fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
                }
                else
                {
                    fs = new FileStream(path, FileMode.Truncate, FileAccess.ReadWrite);
                }

                if (fs == null)
                {
                    return "文件" + path + "创建失败";
                }
                fs.Write(fileBytes, 0, fileBytes.Length);
                fs.Close();
                return "OK";
            }
            catch
            {
                return "fileSaveCurDir " + path + " err";
            }
        }
    }
}
