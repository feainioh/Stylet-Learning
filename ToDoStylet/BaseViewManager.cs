/**************************************************************************
*   
*   =================================
*   CLR版本     ：4.0.30319.42000
*   命名空间    ：ToDoStylet
*   文件名称    ：StudentViewManager.cs
*   =================================
*   创 建 者    ：LQZ
*   创建日期    ：2019-8-14 14:54:05 
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToDoStylet
{
    /// <summary>
    /// 自定义特性，用于跨项目绑定前后台
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed  class ViewModelAttribute : Attribute
    {
        readonly Type viewModel;

        public ViewModelAttribute(Type viewModel)
        {
            this.viewModel = viewModel;
        }

        public Type ViewModel
        {
            get { return viewModel; }
        }
    }

    /// <summary>
    /// 绑定view与viewmodel
    /// </summary>
    public class BaseViewManager : ViewManager
    {
        // 用于存储viewmodel与view类型的字典；Dictionary of ViewModel type -> View type
        private readonly Dictionary<Type, Type> viewModelToViewMapping;

        public BaseViewManager(ViewManagerConfig config)
            : base(config)
        {
            
            var mappings = from type in this.ViewAssemblies.SelectMany(x => x.GetExportedTypes())
                           let attribute = type.GetCustomAttribute<ViewModelAttribute>()
                           where attribute != null && typeof(UIElement).IsAssignableFrom(type)
                           select new { View = type, ViewModel = attribute.ViewModel };

            this.viewModelToViewMapping = mappings.ToDictionary(x => x.ViewModel, x => x.View);
        }
        //根据viewmodel定位view
        protected override Type LocateViewForModel(Type modelType)
        {
            Type viewType;
            if (!this.viewModelToViewMapping.TryGetValue(modelType, out viewType))
                throw new Exception(String.Format("Could not find View for ViewModel {0}", modelType.Name));
            return viewType;
        }
    }
}
