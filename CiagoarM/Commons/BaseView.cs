using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CiagoarM.Commons
{
    public class BaseView : UserControl
    {
        /// <summary>
        /// Dependency Property 위한 래퍼 속성 MyFruit 생성
        /// </summary>
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// 의존프로퍼티 값 등록
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MainWindow));
    }
}
