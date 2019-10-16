/**************************************************************************
*   
*   =================================
*   CLR版本    ：4.0.30319.42000
*   命名空间    ：ToDoStylet.Pages
*   文件名称    ：StudentView.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-8-14 14:32:56 
*   功能描述    ：
*   =================================
*   修改者    ：
*   修改日期    ：
*   修改内容    ：
*   =================================
*  
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToDoStylet.ViewModel;

namespace ToDoStylet.Pages
{
    /// <summary>
    /// StudentView.xaml 的交互逻辑
    /// </summary>
    [ViewModel(typeof(StudentViewModel))]
    public partial class StudentView : Window
    {
        public StudentView()
        {
            InitializeComponent();
        }
    }
}
