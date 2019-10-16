/**************************************************************************
*   
*   =================================
*   CLR版本     ：4.0.30319.42000
*   命名空间    ：ToDoStylet.ViewModel
*   文件名称    ：StudentViewModel.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-8-14 9:37:53 
*   功能描述    ：
*   =================================
*   修 改 者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/
using Autofac;
using Autofac.Extras.DynamicProxy2;
using KingAOP;
using KingAOP.Aspects;
using Stylet;
using StyletIoC;
using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using ToDoStylet.Model;
using Expression = System.Linq.Expressions.Expression;

namespace ToDoStylet.ViewModel
{
    /// <summary>
    /// 后台代码-ViewModel
    /// </summary>
    public class StudentViewModel : Screen
    {

        /// <summary>
        /// Model集合，用于绑定前台ListView
        /// </summary>
        public IObservableCollection<StudentModel> studentModels { get; private set; }
        //事件管理
        private IEventAggregator eventAggregator;

        private StudentModel studentModel;
        /// <summary>
        /// 用于显示被选中的model，绑定前台listview的被选中项
        /// </summary>
        public StudentModel SeletctStudentModel
        {
            get { return this.studentModel; }
            set { SetAndNotify(ref this.studentModel, value); }
        }


        public StudentViewModel(IEventAggregator aggregator)
        {
            this.DisplayName = "Student-Detail";
            this.studentModels = new BindableCollection<StudentModel>();
            this.studentModels.Add(new StudentModel() { ST_Name = "IRON", Gender = "M", Age = 13 });
            this.studentModels.Add(new StudentModel() { ST_Name = "Stylet", Gender = "FM", Age = 3 });
            //设置选中项
            this.SeletctStudentModel = this.studentModels.FirstOrDefault();
            //订阅
            eventAggregator = aggregator;
            Subscriber subscriber = new Subscriber(eventAggregator);

            this.Closed += StudentViewModel_Closed;


        }

        private void StudentViewModel_Closed(object sender, CloseEventArgs e)
        {
            //throw new Exception();
            Console.WriteLine("Close StudentView");
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddStudentModel(object sender, EventArgs e)
        {
            //新增model
            this.studentModels.Add(new StudentModel() { ST_Name = "Unnamed", Gender = "N", Age = 0 });
            //测试AOP
            TestLogger log = new TestLogger();
            //kingAOP必须使用dynamic才能切入
            dynamic entity = new StudentModel { ST_Name = "Jon", Age = 99, Gender = "wang"};
            log.LoginValdate(entity);

            //测试AUTOFAC
            var builder = new ContainerBuilder();
            //注册拦截器到容器
            builder.RegisterType<LoggerHelper>();
            builder.RegisterType<TestInter>();
            builder.Register(c => new TestInter()).As<TestInterface>().EnableInterfaceInterceptors();
            using (var container = builder.Build())
            {
                var test = container.Resolve<TestInter>();
                test.TestIntercept();
                test.Test();
            }
        }
        /// <summary>
        /// 传参方法
        /// </summary>
        /// <param name="item">model参数</param>
        public void RemoveStudent(StudentModel item)
        {
            //去除model
            this.studentModels.Remove(item);
        }
        /// <summary>
        /// 传参方法
        /// </summary>
        /// <param name="name">字符串参数</param>
        public void ParmeterAlert(string name)
        {
            Console.WriteLine(name + " has been selected");
        }

        public bool CanShowMessage
        {
            get { return this.SeletctStudentModel.ST_Name != "Unnamed"; }
        }


        public void ShowMessage()
        {
            ShellViewModel shell = new ShellViewModel(ShellViewModel.GlobalWindowManager, eventAggregator);
            //将view当做对话框弹出
            // this.windowManager.ShowDialog(shell);
            //将view当做窗体弹出
            ShellViewModel.GlobalWindowManager.ShowWindow(shell);

        }

    }


    public class TestLogger : IDynamicMetaObjectProvider
    {
        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new AspectWeaver(parameter, this);
        }

        public void Test()
        {
            Console.Write("Test");
        }
        //添加登录切面
        [LoggerAspect]
        public void LoginValdate(StudentModel entity)
        {
            //只需进行业务逻辑处理,无需进行日志处理
            if (entity.Age == 20 && entity.Gender == "wang")
            {
                entity.ST_Name = "Logged";
            }
            else
            {
                entity.ST_Name = "Error";
            }
        }
    }

    [Intercept(typeof(LoggerHelper))]
    public class TestInter:TestInterface
    {
        public virtual void TestIntercept()
        {
            Console.WriteLine("This is a Testing");
        }

       public void Test()
        {
            Console.WriteLine("This is a Testing");
        }
    }
}
