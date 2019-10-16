/**************************************************************************
*   
*   =================================
*   CLR版本     ：4.0.30319.42000
*   命名空间    ：ToDoStylet.ViewModel
*   文件名称    ：ShellViewModel.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-8-14 13:10:13 
*   功能描述    ：
*   =================================
*   修 改 者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/
using Stylet;
using StyletIoC;
using System;
using System.Dynamic;
using System.Threading;
using System.Windows;

namespace ToDoStylet.ViewModel
{
    public  class ShellViewModel:Screen
    {

        public static IWindowManager GlobalWindowManager;
        IEventAggregator eventAggregator;
        public ShellViewModel(IWindowManager window,IEventAggregator aggregator)
        {
            GlobalWindowManager = window;
            //广播
            eventAggregator = aggregator;
            //窗口关闭事件
            this.Closed += ShellViewModel_Closed;
        }

        private string state="Loading";
        //绑定前台显示内容
        public string NowState
        {
            get { return this.state; }
            set { SetAndNotify(ref this.state, value); }
        }
        
        public void OpenStudentWindow()
        {
            //弹出窗口
            StudentViewModel studentView = new StudentViewModel(eventAggregator);
            GlobalWindowManager.ShowWindow(studentView);
            //当做对话框弹出调用关闭方法
            //this.RequestClose(true);          
        }
        //广播方法
        public void Publish()
        {
            //事件广播
            Publisher publisher = new Publisher(eventAggregator);
            publisher.PublishEvent();
            Thread thread = new Thread(Test);
            thread.IsBackground = true;
            thread.Start();
          
        }

        private void Test()
        {
            //在后台线程中调用UI线程
            Execute.OnUIThread(new Action(() => {
                NowState = "OnUIThread";
            }));
            Thread.Sleep(1000);
            Execute.PostToUIThread(new Action(() => {
                NowState = "PostToUIThread";
            }));
        }
        
        /// <summary>
        /// 绑定WPF自带的窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShellViewModel_Closed(object sender, CloseEventArgs e)
        {
            MessageBox.Show("View Closed");
            
        }
   
        public void Open(object sender,EventArgs e)
        {
            Console.WriteLine("View Open");
        }

    }
}
