using System.Windows;
using ToDoStylet.ViewModel;

namespace ToDoStylet.Pages
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    [ViewModel(typeof(ShellViewModel))]
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }
    }
}
