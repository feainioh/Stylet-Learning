/**************************************************************************
*   
*   =================================
*   CLR版本     ：4.0.30319.42000
*   命名空间    ：ToDoStylet.ViewModel
*   文件名称    ：GetMessages.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-10-9 9:10:19 
*   功能描述    ：
*   =================================
*   修 改 者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/
using Castle.DynamicProxy;
using Stylet;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToDoStylet.ViewModel
{
    //订阅事件
    public class MyEvent
    {
        public MyEvent()
        {
            MessageBox.Show("Init Event");
        }        
        public bool Messages()
        {
            MessageBox.Show("Get Message: "+DateTime.Now.ToString("HH:mm:ss.fff"));
            return true;
        }
    }
    //订阅
    public class Subscriber : IHandle<MyEvent>
    {
        public Subscriber(IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
        }
        public void Handle(MyEvent message)
        {
            message.Messages();
        }
    }
    //广播
    class Publisher
    {
        private IEventAggregator eventAggregator;
        public Publisher(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void PublishEvent()
        {
            this.eventAggregator.Publish(new MyEvent().Messages());
        }

        public void PublishEventWithChannels()
        {
            this.eventAggregator.Publish(new MyEvent(),"ChannelA","ChannelB");
        }
    }    


    public  interface TestInterface
    {
        void Test();
    }

}
