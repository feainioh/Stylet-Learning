using System;
using System.Reflection;
using KingAOP.Aspects;
using Stylet;
using StyletIoC;
using ToDoStylet.Pages;
using ToDoStylet.ViewModel;

namespace ToDoStylet
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            //base.ConfigureIoC(builder);
            //获取所有程序集
            var ass = System.AppDomain.CurrentDomain.GetAssemblies();
            //遍历程序集，将需要的程序集注册到容器中
            foreach (Assembly assembly in ass)
            {
                if (assembly.FullName.Contains("ToDoStyle") )
                {

                    builder.Assemblies.Add(assembly);
                }
            }
            //builder.Bind<Screen>().To<StudentViewModel>().InSingletonScope();
            //注册视图管理器
            builder.Bind<IViewManager>().To<BaseViewManager>();
        }

        protected override void OnStart()
        {
            //启用框架自带日志
            Stylet.Logging.LogManager.LoggerFactory = name => new LoggerAspect(name);
            Stylet.Logging.LogManager.Enabled = true;
            
        }
    }
}
