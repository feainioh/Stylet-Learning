using Autofac;
using Autofac.Extras.DynamicProxy2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPConsole
{
    [System.Security.SecuritySafeCritical]
    class Program
    {
        static void Main(string[] args)
        {
            //启用拦截器主要有两个方法：EnableInterfaceInterceptors()，EnableClassInterceptors()
            //EnableInterfaceInterceptors方法会动态创建一个接口代理
            //EnableClassInterceptors方法会创建一个目标类的子类代理类，这里需要注意的是只会拦截虚方法，重写方法
            //注意：需要引用Autofac.Extras.DynamicProxy2才能使用上面两个方法      
            #region 启用接口代理拦截(推荐用这种方式)
            //创建拦截容器
            var builder2 = new ContainerBuilder();
            //注册拦截器到容器
            builder2.RegisterType<LogInterceptor>();
            //构造函数注入(只要调用者传入实现该接口的对象，就实现了对象创建，下面两种方式)
            builder2.RegisterType<PersonManager>();
            //方式一：给类型上加特性Attribute
            //属性注入
            builder2.Register(c => new Man { Age = "20" }).As<IPerson>().EnableInterfaceInterceptors();
            //builder2.RegisterType<Man>().As<IPerson>().EnableInterfaceInterceptors();
            builder2.RegisterType<Woman>().Named<IPerson>("Woman").EnableInterfaceInterceptors();
            //方式二：在注册类型到容器的时候动态注入拦截器(去掉类型上的特性Attribute)
            //builder2.RegisterType<Man>().As<IPerson>().InterceptedBy(typeof(LogInterceptor)).EnableInterfaceInterceptors();
            //builder2.RegisterType<Woman>().Named<IPerson>("Woman").InterceptedBy(typeof(LogInterceptor)).EnableInterfaceInterceptors();    
            using (var container = builder2.Build())
            {
                //从容器获取对象
                var Manager = container.Resolve<PersonManager>();
                Manager.Say("管理员");
                var Person = container.Resolve<IPerson>();
                Person.Say("张三");
                var Woman = container.ResolveNamed<IPerson>("Woman");
                Woman.Say("王萌");
            }
            Console.ReadLine();
            #endregion            
        }
    }
}
