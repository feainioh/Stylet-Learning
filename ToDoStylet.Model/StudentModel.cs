/**************************************************************************
*   
*   =================================
*   CLR版本     ：4.0.30319.42000
*   命名空间    ：ToDoStylet.Model
*   文件名称    ：StudentModel.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-8-14 8:42:33 
*   功能描述    ：
*   =================================
*   修 改 者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*   
***************************************************************************/
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoStylet.Model
{
    /// <summary>
    /// 后台属性
    /// </summary>
    public class StudentModel: PropertyChangedBase
    {
        private string st_Name;
        //姓名
        public string ST_Name
        {
            get { return this.st_Name; }
            set { this.SetAndNotify(ref this.st_Name, value); }
        }

        private string gender;
        //性别
        public string Gender
        {
            get { return gender; }
            set { this.SetAndNotify(ref this.gender, value); }
        }

        private int age;
        //年龄
        public int Age
        {
            get { return age; }
            set { this.SetAndNotify(ref this.age,value); }
        }

    }
}
