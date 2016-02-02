using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AvonManager.Common.Controls
{
    public class SvgButton : Button
    {

        public PathFigureCollection Figures
        {
            get { return (PathFigureCollection)GetValue(FiguresProperty); }
            set { SetValue(FiguresProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Figures.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiguresProperty =
            DependencyProperty.Register("Figures", typeof(PathFigureCollection), typeof(SvgButton), new PropertyMetadata(null));



        public DataTemplate Child
        {
            get { return (DataTemplate)GetValue(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Child.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register("Child", typeof(DataTemplate), typeof(SvgButton), new PropertyMetadata(null));


    }
}
