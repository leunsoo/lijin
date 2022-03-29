using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Xml;

namespace MysqlLib
{
    class Util
    {
    }

    public struct Config  //프로그램 전체에서 사용하는 고정형자료
    {
        // public static string conn1= "SERVER=175.207.12.133;Database=main_db;User Id=pera;Password=peras(@(&;character set=euckr;allow zero datetime=yes";
        public static string conn = "";
        static public string cfgFile = "pConfig.xml";    // db설정파일(Master)
        static public string URL = null;    // xml 위치
        static private byte[] Skey = Encoding.UTF8.GetBytes("3356B0AC");
        //db 연결 문자열 master
        static public string DBSERVER = "";
        static public string DBPORT = "";
        static public string DBNAME = "";
        static public string DBUSER = "";
        static public string DBPASSWD = "";

        static public string UserId = "";
        static public string UserName = "";

        static public string[] array_txt = null;   // 샹위메뉴 id
        static public string[] array_path = null;   // 메뉴 경로

        static public string Encrypt(string pData)
        {
            if (pData == null || pData == "") { return ""; }
            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
            desc.Key = Skey;
            desc.IV = Skey;

            MemoryStream ms = new MemoryStream();
            CryptoStream cryStream = new CryptoStream(ms, desc.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] data = Encoding.UTF8.GetBytes(pData.ToCharArray());
            cryStream.Write(data, 0, data.Length);
            cryStream.FlushFinalBlock();
            string strRTN = Convert.ToBase64String(ms.ToArray());
            desc.Clear();
            ms.Dispose();
            cryStream.Dispose();
            return strRTN;
        }

        // 복호화
        static public string Decrypt(string sData)
        {
            if (sData == null || sData == "") { return ""; }
            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
            desc.Key = Skey;
            desc.IV = Skey;
            byte[] data = Convert.FromBase64String(sData);

            MemoryStream ms = new MemoryStream(data);
            CryptoStream cryStream = new CryptoStream(ms, desc.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cryStream);
            string strRTN = sr.ReadToEnd();
            desc.Clear();
            ms.Dispose();
            cryStream.Dispose();
            sr.Dispose();
            return strRTN;
        }

        // ===========  문자열, 키 값이용 암호화  =======================
        static private string EncryptKey(string pData, string argKEY)
        {
            if (pData == null || pData == "") { return ""; }
            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
            desc.Key = Encoding.UTF8.GetBytes(argKEY);
            desc.IV = Encoding.UTF8.GetBytes(argKEY);

            MemoryStream ms = new MemoryStream();
            CryptoStream cryStream = new CryptoStream(ms, desc.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] data = Encoding.UTF8.GetBytes(pData.ToCharArray());
            cryStream.Write(data, 0, data.Length);
            cryStream.FlushFinalBlock();
            string strRTN = Convert.ToBase64String(ms.ToArray());
            desc.Clear();
            ms.Dispose();
            cryStream.Dispose();
            return strRTN;
        }

        // ======   문자열 복호화(Key변경) ==================
        static public string DecryptKey(string sData, string argKey)
        {
            if (sData == null || sData == "") { return ""; }
            DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
            desc.Key = Encoding.UTF8.GetBytes(argKey);
            desc.IV = Encoding.UTF8.GetBytes(argKey);

            byte[] data = Convert.FromBase64String(sData);

            MemoryStream ms = new MemoryStream(data);
            CryptoStream cryStream = new CryptoStream(ms, desc.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cryStream);
            string strRTN = sr.ReadToEnd();

            desc.Clear();
            ms.Dispose();
            cryStream.Dispose();
            sr.Dispose();
            return strRTN;
        }

        // 환경설정 읽어오기
        static public bool getConfig()
        {
            string url = URL; //  Environment.CurrentDirectory + "\\" + cfgFile;

            if (!File.Exists(url)) { return false; }
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(url);

                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList nodes = root.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    switch (node.Name)
                    {
                        case "DBSERVER":
                            DBSERVER = Decrypt(node.InnerText);
                            break;
                        case "DBNAME":
                            DBNAME = Decrypt(node.InnerText);
                            break;
                        case "DBUSER":
                            DBUSER = Decrypt(node.InnerText);
                            break;
                        case "DBPASSWD":
                            DBPASSWD = Decrypt(node.InnerText);
                            break;
                        case "DBPORT":
                            DBPORT = Decrypt(node.InnerText);
                            break;
                    }
                }
                string strIp = DBSERVER;
                strIp = DBSERVER;
                if (!string.IsNullOrEmpty(DBPORT)) strIp = string.Format("{0},{1}", DBSERVER, DBPORT);

                conn = string.Format("Server={0};Database={1};Uid={2};Pwd={3};character set=euckr;" +
                    "allow zero datetime=yes", DBSERVER, DBNAME, DBUSER, DBPASSWD);

                return true;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("getConfig Error : " + ex.Message);
                return false;
            }
        }

        // 환경설정 저장
        static public bool setConfig(string strIP, string port, string strName, string strUser, string strPasswd)
        {
            string url = Environment.CurrentDirectory + "\\" + cfgFile;

            try
            {
                XmlTextWriter xWriter = new XmlTextWriter(url, Encoding.UTF8);
                xWriter.Formatting = System.Xml.Formatting.Indented;   // 들여쓰기 적용
                xWriter.WriteStartDocument();               // 쓰기 시작 File.Open과 같은 메서드
                xWriter.WriteStartElement("cfg");           // 엘리먼트 쓰기

                xWriter.WriteStartElement("DBSERVER");
                xWriter.WriteString(Encrypt(strIP));
                xWriter.WriteEndElement();

                xWriter.WriteStartElement("DBNAME");                // 데이터 베이스 네임
                xWriter.WriteString(Encrypt(strName));
                xWriter.WriteEndElement();

                xWriter.WriteStartElement("DBUSER");                // 데이터베이스 사용자 계정
                xWriter.WriteString(Encrypt(strUser));
                xWriter.WriteEndElement();

                xWriter.WriteStartElement("DBPASSWD");              // 계정 패스워드
                xWriter.WriteString(Encrypt(strPasswd));
                xWriter.WriteEndElement();

                xWriter.WriteStartElement("DBPORT");              // port
                xWriter.WriteString(Encrypt(port));
                xWriter.WriteEndElement();

                xWriter.WriteEndDocument();                         // 문서 끝 
                xWriter.Close();                                   // 객체 닫기
                return true;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("setConfig Error : " + ex.Message);
                return false;
            }
        }
    }
}
