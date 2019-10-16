/**************************************************************************
*   
*   =================================
*   CLR版本     ：4.0.30319.42000
*   命名空间    ：AOPConsole
*   文件名称    ：LoggerAspect.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-10-14 8:57:19 
*   功能描述    ：
*   =================================
*   修 改 者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/
using Autofac.Extras.DynamicProxy2;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPConsole
{
    class LogInterceptor : IInterceptor
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

    /// <summary>
    /// 定义一个接口
    /// </summary>
    public interface IPerson
    {
        void Say(string Name);
    }

    /// <summary>
    /// 继承接口，并实现方法，给类型加上特性Attribute
    /// </summary>
    [Intercept(typeof(LogInterceptor))]
    public class Man : IPerson
    {
        public string Age;

        public void Say(string Name)
        {
            Console.WriteLine("男人调用Say方法！姓名：" + Name + "，年龄：" + Age);
        }
    }

    /// <summary>
    /// 继承接口，并实现方法，给类型加上特性Attribute
    /// </summary>
    [Intercept(typeof(LogInterceptor))]
    public class Woman : IPerson
    {
        public void Say(string Name)
        {
            Console.WriteLine("女人调用Say方法！姓名：" + Name);
        }
    }

    /// <summary>
    /// 管理类
    /// </summary>
    public class PersonManager
    {
        IPerson _Person;

        /// <summary>
        /// 根据传入的类型动态创建对象
        /// </summary>
        /// <param name="ds"></param>
        public PersonManager(IPerson Person)
        {
            _Person = Person;
        }

        public void Say(string Name)
        {
            _Person.Say(Name);
        }
    }
}
