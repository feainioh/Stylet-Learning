/**************************************************************************
*   
*   =================================
*   CLR版本     ：4.0.30319.42000
*   命名空间    ：ToDoStylet.ViewModel
*   文件名称    ：LogHelper.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-10-9 16:31:19 
*   功能描述    ：日志操作类-使用KingAOP框架进行切面编程
*   =================================
*   修 改 者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/
using Castle.DynamicProxy;
using KingAOP.Aspects;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ToDoStylet.ViewModel
{
    public class LoggerAspect : OnMethodBoundaryAspect, Stylet.Logging.ILogger
    {
        // 日志文件存放目录
        private string _logDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log"; 
        //流程日志文件存放目录
        private string _commLogDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\CommLog";
        //异常日志文件存放目录
        private string _errLogDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\ErrLog";
        //传入的操作源名称
        private readonly string name;
        public LoggerAspect(string loggerName)
        {    
            this.DelOldFile();
            name = loggerName;
        }
        public LoggerAspect(){}

        //日志级别为明细
        public void Info(string format, params object[] args)
        {
            if (this.name == "Stylet.ViewManager" || this.name == "Stylet.WindowManager") return;
            var logStr = new StringBuilder();
            logStr.AppendLine();
            logStr.Append("日志时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            logStr.AppendLine();
            logStr.Append("日志级别：[INFO]");
            logStr.AppendLine();
            logStr.Append(string.Format("操作对象：[{0}]", this.name));
            logStr.AppendLine();
            logStr.Append(String.Format("日志内容：{0}", String.Format(format, args)));
            //写到本地
            WriteLog(logStr.ToString(), _commLogDir);
        }
        //日志级别为警告，同明细
        public void Warn(string format, params object[] args)
        {
            if (this.name == "Stylet.ViewManager") return;
            var logStr = new StringBuilder();
            logStr.AppendLine();
            logStr.Append("日志时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            logStr.AppendLine();
            logStr.Append("日志级别：[WARN]");
            logStr.AppendLine();
            logStr.Append(string.Format("操作对象：[{0}]", this.name));
            logStr.AppendLine();
            logStr.Append(String.Format("日志内容： {0}", String.Format(format, args)));
            //写到本地
            WriteLog(logStr.ToString(), _commLogDir);
        }
        //日志级别为异常
        public void Error(Exception exception, string message = null)
        {
            var logStr = new StringBuilder();
            logStr.AppendLine();
            logStr.Append("日志时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            logStr.AppendLine();
            logStr.Append("日志级别：[ERROR]");
            logStr.AppendLine();
            logStr.Append(string.Format("操作对象：[{0}]", this.name));
            logStr.AppendLine();
            if (message == null)
                logStr.Append(String.Format("日志内容：{0}", exception));
            else
                logStr.Append(String.Format("日志内容：{0} {1}", message, exception));
            //写到本地
            WriteLog(logStr.ToString(), _errLogDir);
        }

        /// <summary>
        /// 删除过期文件
        /// </summary>
        private void DelOldFile()
        {
            // 遍历指定文件夹下所有子文件，将一定期限前的日志文件删除。
            if (!Directory.Exists(this._logDir))
            {
                // 如果文件夹目录不存在
                Directory.CreateDirectory(this._logDir);
            }
            if (!Directory.Exists(this._commLogDir))
            {
                Directory.CreateDirectory(this._commLogDir);
            }
            if (!Directory.Exists(this._errLogDir))
            {
                Directory.CreateDirectory(this._errLogDir);
            }

            var vFiles = (new DirectoryInfo(this._logDir)).GetFiles();
            for (int i = vFiles.Length - 1; i >= 0; i--)
            {
                // 指定条件，然后删除
                if (vFiles[i].Name.Contains("Log"))
                {
                    if ((DateTime.Now - vFiles[i].LastWriteTime).Days > 14)
                    {
                        vFiles[i].Delete();
                    }
                }
            }

        }
        #region
        public override void OnEntry(MethodExecutionArgs args)
        {
            string logData = CreateInfoLogData("Entering", args);
            //写入本地
            WriteLog(logData,_commLogDir);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            string logData = CreateErrLogData("Exception", args);
            WriteLog(logData, _errLogDir);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            string logData = CreateInfoLogData("Success", args);
            //写入本地
            WriteLog(logData, _commLogDir);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string logData = CreateInfoLogData("Exiting", args);
            //写入本地
            WriteLog(logData, _commLogDir);
        }

        /// <summary>
        /// AOP处理逻辑,日志等级为INFO
        /// </summary>
        /// <param name="methodStage">方法状态</param>
        /// <param name="args">方法参数</param>
        /// <returns></returns>
        private string CreateInfoLogData(string methodStage, MethodExecutionArgs args)
        {
            var str = new StringBuilder();
            str.AppendLine();
            str.Append("日志时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            str.AppendLine();
            str.Append("日志级别：[INFO]");
            str.AppendLine();
            str.Append(string.Format("操作对象：[{0}] -->> {1} ", args.Method, methodStage));
            str.AppendLine();
            str.Append("日志内容：");
            foreach (var argument in args.Arguments)
            {
                //下面利用反射机制获取对象名称和对象属性和属性值
                var argType = argument.GetType();

                str.Append("对象类型："+argType.Name+" - 对象属性：");

                if (argType == typeof(string) || argType.IsPrimitive)
                {
                    str.Append(argument);
                }
                else
                {
                    foreach (var property in argType.GetProperties())
                    {
                        str.AppendFormat("{0} = {1} ; ",
                            property.Name, property.GetValue(argument, null));
                    }
                }
            }
            return str.ToString();
        }
        /// <summary>
        /// AOP处理逻辑,日志等级为Error
        /// </summary>
        /// <param name="methodStage">方法状态</param>
        /// <param name="args">方法参数</param>
        /// <returns></returns>
        private string CreateErrLogData(string methodStage,MethodExecutionArgs args)
        {
            var str = new StringBuilder();
            str.AppendLine();
            str.Append("日志时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            str.AppendLine();
            str.Append("日志级别：[ERROR]");
            str.AppendLine();
            str.Append(string.Format("对象类型：[{0}] -->> {1} ", args.Method, methodStage));
            str.AppendLine();
            str.Append("日志内容：");
            foreach (var argument in args.Arguments)
            {
                //下面利用反射机制获取对象名称和对象属性和属性值
                var argType = argument.GetType();

                str.Append("操作对象：" + argType.Name + " - 对象属性：");

                if (argType == typeof(string) || argType.IsPrimitive)
                {
                    str.Append(argument);
                }
                else
                {
                    foreach (var property in argType.GetProperties())
                    {
                        str.AppendFormat("{0} = {1} ;",
                            property.Name, property.GetValue(argument, null));
                    }
                }
            }
            return str.ToString();
        }
        #endregion

        /// <summary>
        /// 写入一条日志记录
        /// </summary>
        /// <param name="pLog">日志记录内容</param>
        private void WriteLog(string pLog, string path)
        {
            lock (path) //排它锁：防止主程序中出现多线程同时访问同一个文件出错
            {
                // 根据时间创建一个日志文件
                var vDT = DateTime.Now;
                string vLogDir = string.Format("{0}\\{1}{2}{3}", path, vDT.Year, vDT.Month, vDT.Day);
                //创建子目录
                if (!Directory.Exists(vLogDir)) Directory.CreateDirectory(vLogDir);
                string vLogFile = string.Format("{0}\\{1}.log", vLogDir, vDT.ToString("yyyyMMddHH"));
                // 创建文件流，用于写入
                using (FileStream fs = new FileStream(vLogFile, FileMode.Append))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(pLog);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
        }
    }

   public class LoggerHelper : IInterceptor
    {
        /// <summary>
        /// 拦截方法 打印被拦截的方法执行前的名称、参数和方法执行后的 返回结果
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("方法执行前:拦截{0}类下的方法{1}的参数是{2}",
                invocation.InvocationTarget.GetType(),
                invocation.Method.Name, string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            //在被拦截的方法执行完毕后 继续执行
            invocation.Proceed();

            Console.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
            Console.WriteLine();
        }
    }


}
