namespace QuickEditor.ToolKit
{
    using System;
    using System.IO;
    using System.Text;
    using UnityEngine;

    public class FileEncodingUtils
    {
        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>是否是带签名的UTF8编码</returns>
        public static bool IsUTF8_BOM(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            r.Close();
            fs.Close();

            return IsUTF8_BOMBytes(ss);
        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;    //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }

            if (charByteCounter > 1)
            {
                Debug.LogError("非预期的byte格式");
            }

            return true;
        }

        /// <summary>
        /// 判断是否是带 BOM 的 UTF8 格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8_BOMBytes(byte[] data)
        {
            if (data.Length < 3)
                return false;

            return ((data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF));
        }

        public static Encoding GetEncoding(FileStream fs)
        {
            Encoding reVal = Encoding.Default;

            BinaryReader bReader = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] buffer = bReader.ReadBytes(i);
            return GetEncoding(buffer);
        }

        public static Encoding GetEncoding(byte[] buffer)
        {
            Encoding reVal = Encoding.Default;

            if (IsUTF8Bytes(buffer) || IsUTF8_BOMBytes(buffer))
            {
                reVal = Encoding.UTF8;
            }
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF && buffer[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE && buffer[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }

            return reVal;
        }

        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }

        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }

        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                //根据文件流的前4个字节判断Encoding
                //Unicode {0xFF, 0xFE};
                //BE-Unicode {0xFE, 0xFF};
                //UTF8 = {0xEF, 0xBB, 0xBF};
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
                {
                    targetEncoding = Encoding.UTF8;
                }
                //恢复Seek位置
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }

        // 新增加一个方法，解决了不带BOM的 UTF8 编码问题
        public static Encoding GetEncoding(Stream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            byte[] ss = r.ReadBytes(4);
            if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            else
            {
                if (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
                {
                    reVal = Encoding.UTF8;
                }
                else
                {
                    int i;
                    int.TryParse(fs.Length.ToString(), out i);
                    ss = r.ReadBytes(i);

                    if (IsUTF8Bytes(ss))
                        reVal = Encoding.UTF8;
                }
            }
            r.Close();
            return reVal;
        }

        public static void SetFileEncoding(string sourceFile, Encoding targetEncoding)
        {
            if (!File.Exists(sourceFile))
            {
                Debug.Log(string.Format("File -> {0} is Not Exists", sourceFile));
                return;
            }
            Encoding fileEncoding = GetEncoding(sourceFile);
            string content = File.ReadAllText(sourceFile, fileEncoding);
            File.WriteAllText(sourceFile, content, targetEncoding);
        }

        public static void CovertToUTF8_BOM(string sourceFile)
        {
            byte[] BomHeader = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            FileStream fs = new FileStream(sourceFile, FileMode.Open, FileAccess.ReadWrite);

            //按默认编码获取文件内容
            BinaryReader r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            r.Close();

            bool isBom = false;
            if (ss.Length >= 3)
            {
                if (ss[0] == BomHeader[0] && ss[1] == BomHeader[1] && ss[2] == BomHeader[2])
                {
                    isBom = true;
                }
            }

            //将内容转换为UTF8格式，并添加Bom头
            if (!isBom)
            {
                string content = Encoding.Default.GetString(ss);
                byte[] newSS = Encoding.UTF8.GetBytes(content);

                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(BomHeader, 0, BomHeader.Length);
                fs.Write(newSS, 0, i);
            }

            fs.Close();
        }

        /// <summary>
        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// </summary>
        /// <param name="FILE_NAME">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetType(string FILE_NAME)
        {
            FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        /// <summary>
        /// 通过给定的文件流，判断文件的编码类型
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetType(FileStream fs)
        {
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || IsUTF8_BOMBytes(ss))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();

            return reVal;
        }

        /// <summary>
        /// 通过给定的字节数组，判断其编码类型
        /// </summary>
        /// <param name="ss">字节数组</param>
        /// <returns>字节数组的编码类型</returns>
        public static Encoding GetType(byte[] ss)
        {
            Encoding reVal = Encoding.Default;

            if (IsUTF8Bytes(ss) || IsUTF8_BOMBytes(ss))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }

            return reVal;
        }
    }
}