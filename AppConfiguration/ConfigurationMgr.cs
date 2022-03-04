using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Globals;
using MysqlLib;

namespace AppConfiguration
{
    public class ConfigurationMgr
    {
        private string EnKey;

        #region Properties
        public string DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public string CustomerName { get; set; }
        public bool Evaluation { get; set; }
        public DateTime? EndOfEvaluation { get; set; } = null;
       // public int UserId { get; set; }
        public string LoginId { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public string LoginComcode { get; set; }
        public string LoginComname { get; set; }

        //public IDatabase DbConnector { get; set; }
        //public UserModel UserModel { get; set; }
        public byte[] SHA_SAULT { get; private set; }
        public ePmsEnum Language { get; set; }
        #endregion


        private static ConfigurationMgr instance;
        protected ConfigurationMgr()
        {
            LoadConfiguration();
        }

        public static ConfigurationMgr Instance()
        {
            if (instance == null)
            {
                instance = new ConfigurationMgr();
            }
            return instance;
        }

        private void LoadConfiguration()
        {
            try
            {
                Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConnectionString = Config.conn; //  currentConfig.ConnectionStrings.ConnectionStrings["ePMSConnection"].ConnectionString;
                EnKey = currentConfig.AppSettings.Settings["EnKey"].Value;
                DatabaseType = currentConfig.AppSettings.Settings["DatabaseType"].Value;
                switch (currentConfig.AppSettings.Settings["Language"].Value)
                {
                    case "Korean":
                    case "korean":
                        Language = ePmsEnum.Korean;
                        break;
                    case "English":
                    case "english":
                        Language = ePmsEnum.English;
                        break;
                    default:
                        Language = ePmsEnum.Korean;
                        break;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public void SaveConfiguration()
        {
            try
            {
                Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                switch (Language)
                {
                    case ePmsEnum.Korean:
                        currentConfig.AppSettings.Settings["Language"].Value = "Korean";
                        break;
                    case ePmsEnum.English:
                        currentConfig.AppSettings.Settings["Language"].Value = "English";
                        break;
                    default:
                        currentConfig.AppSettings.Settings["Language"].Value = "Korean";
                        break;
                }
                currentConfig.AppSettings.Settings["DatabaseType"].Value = DatabaseType;

                currentConfig.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
