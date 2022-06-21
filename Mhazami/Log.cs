using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mhazami
{
    public static class Log
    {
        internal class LogDTO
        {
            public string Message { get; set; }
            public LogType LogType { get; set; }
            public string SubSystem { get; set; }
            public string StackTrace { get; set; }
            public DateTime DateTime { get; set; }
        }

        internal static LogSettings Settings
        {
            get
            {
                LogSettings logSettings;
                try
                {
                    logSettings = (LogSettings)ConfigurationManager.GetSection("Azami/log");
                }
                catch
                {
                    logSettings = new LogSettings();
                }
                return logSettings;
            }
        }

        public static void Save(string message, LogType logType = LogType.ApplicationLog, string subSystem = "", string stackTrace = "")
        {
            if (!Settings.Enabled) return;

            if (string.IsNullOrEmpty(stackTrace))
                stackTrace = Environment.StackTrace;
            var logDto = new LogDTO
                             {
                                 Message = message,
                                 LogType = logType,
                                 SubSystem = subSystem,
                                 StackTrace = stackTrace,
                                 DateTime = DateTime.Now
                             };
            switch (Settings.Destination)
            {
                case LogDestination.TextFile:
                    SaveFile(logDto);
                    break;
                case LogDestination.EventLog:
                    SaveEventLog(logDto);
                    break;
                case LogDestination.DataBase:
                    SaveDataBase(logDto);
                    break;
            }
        }

        internal static void SaveDataBase(LogDTO logDto)
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.ConnectionString))
                {
                    SaveFile(logDto);
                    return;
                }
                var connection = new SqlConnection(Settings.ConnectionString);
                var fields = "";
                var value = "";
                foreach (var property in logDto.GetType().GetProperties())
                {
                    fields += string.Format("{0},", property.Name);
                    var propertyValue = property.GetValue(logDto, null).ToString();
                    if (property.PropertyType.FullName != null
                        && (property.PropertyType.FullName.ToLower().Contains("string")
                            || property.PropertyType.IsEnum
                            || property.PropertyType.FullName.ToLower().Contains("datetime")))
                    {
                        propertyValue = propertyValue.Replace("'", "''");
                        value += string.Format("N'{0}',", propertyValue);
                    }
                    else
                        value += string.Format("{0},", propertyValue);
                }
                if (string.IsNullOrEmpty(fields))
                {
                    SaveFile(logDto);
                    return;
                }
                fields += "Id";
                value += string.Format("'{0}'", Guid.NewGuid());
                var commandText = string.Format("INSERT INTO Log.Log({0})VALUES({1})", fields, value);
                var command = new SqlCommand(commandText, connection);
                connection.Open();
                try
                {
                    var result = command.ExecuteNonQuery();
                    if (result == 0)
                    {
                        SaveFile(logDto);
                    }
                }
                catch (Exception ex)
                {
                    SaveFile(logDto);
                    logDto.Message = ex.Message;
                    logDto.StackTrace = ex.StackTrace;
                    logDto.DateTime = DateTime.Now;
                    logDto.SubSystem = "Log";
                    SaveFile(logDto);
                }
                finally
                {
                    connection.Close();
                }

            }
            catch (Exception exp)
            {
                SaveFile(logDto);
                logDto.Message = exp.Message;
                logDto.StackTrace = exp.StackTrace;
                logDto.DateTime = DateTime.Now;
                logDto.SubSystem = "Log";
                SaveFile(logDto);
            }
        }

     

        internal static void SaveFile(LogDTO logDto)
        {
            var path = Settings.LogPath;
            if (string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "log";
            }
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileName = path;
                if (!fileName.EndsWith("\\"))
                    fileName += "\\";
                fileName += string.Format("{0} - {1} - {2}.txt", logDto.LogType, DateTime.Now.ToString().Replace("/", "-").Replace(":", "-"), Guid.NewGuid());

                var fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                var sr = new StreamWriter(fs);
                sr.Write(LogContent(logDto));
                sr.Flush();
                sr.Close();
                fs.Close();
            }
            catch (Exception)
            {
                SaveEventLog(logDto);
            }
        }
        internal static void SaveEventLog(LogDTO logDto)
        {
            try
            {
                const string source = "AzamiLog";
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, "Application");


                var eventLogType = EventLogEntryType.Information;
                switch (logDto.LogType)
                {
                    case LogType.ApplicationError:
                    case LogType.FatalError:
                    case LogType.UnkownException:
                        eventLogType = EventLogEntryType.Error;
                        break;
                    case LogType.UserAction:
                    case LogType.SystemAction:
                    case LogType.AgentLog:
                    case LogType.ApplicationLog:
                    case LogType.ServiceLog:
                        eventLogType = EventLogEntryType.Information;
                        break;
                    case LogType.Warning:
                        eventLogType = EventLogEntryType.Warning;
                        break;
                }
                EventLog.WriteEntry(source, LogContent(logDto), eventLogType);
            }
            catch
            {

            }
        }
        private static string LogContent(LogDTO logDto)
        {
            var str = new StringBuilder();
            str.AppendLine("Date : ");
            str.AppendLine(logDto.DateTime.ToString());
            str.AppendLine();
            str.AppendLine();
            str.AppendLine("Log Type : ");
            str.AppendLine(logDto.LogType.ToString());
            str.AppendLine();
            str.AppendLine();
            str.AppendLine("Message : ");
            str.AppendLine(logDto.Message);
            str.AppendLine();
            str.AppendLine();
            if (!string.IsNullOrEmpty(logDto.SubSystem))
            {
                str.AppendLine("SubSystem : ");
                str.AppendLine(logDto.SubSystem);
                str.AppendLine();
                str.AppendLine();
            }
            str.AppendLine("Stack Trace : ");
            str.AppendLine(logDto.StackTrace);
            str.AppendLine();
            str.AppendLine();
            return str.ToString();
        }
    }

    public class LogSettings : ConfigurationSection
    {
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("destination")]
        public LogDestination Destination
        {
            get { return (LogDestination)this["destination"]; }
            set { this["destination"] = (LogDestination)Enum.Parse(typeof(LogDestination), value.ToString()); }
        }

        [ConfigurationProperty("logPath")]
        public string LogPath
        {
            get { return (string)this["logPath"]; }
            set { this["logPath"] = value; }
        }

        [ConfigurationProperty("connectionString")]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }
    }

    public enum LogType
    {
        ApplicationError,
        UnkownException,
        FatalError,
        UserAction,
        SystemAction,
        AgentLog,
        ApplicationLog,
        ServiceLog,
        Warning
        
    }

    public enum LogDestination
    {
        TextFile,
        EventLog,
        DataBase
    }
}
