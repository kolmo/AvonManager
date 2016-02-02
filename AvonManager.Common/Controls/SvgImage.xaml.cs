using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvonManager.Common.Controls
{
    /// <summary>
    /// Interaction logic for SvgImage.xaml
    /// </summary>
    public partial class SvgImage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
        public SvgImage()
        {
            InitializeComponent();
        }

        #endregion

        #region Child


        public DataTemplate Child
        {
            get { return (DataTemplate)GetValue(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Child.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register("Child", typeof(DataTemplate), typeof(SvgImage), new PropertyMetadata(null, ChildPropertyChangedCallback));

        private static void ChildPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataTemplate element = e.NewValue as DataTemplate;
            SvgImage img = d as SvgImage;
            img.presenter.ContentTemplate = element;
        }


        #endregion
        #region Figure

        /// <summary>
        /// The figure property
        /// </summary>
        public static readonly DependencyProperty FigureProperty = DependencyProperty.Register("Figure", typeof(PathFigureCollection), typeof(SvgImage), new PropertyMetadata(null, FigurePropertyChangedCallback));

        /// <summary>
        /// Gets or sets the figure.
        /// </summary>
        /// <value>
        /// The figure.
        /// </value>
        public PathFigureCollection Figure
        {
            get
            {
                return (PathFigureCollection)GetValue(FigureProperty);
            }

            set
            {
                SetValue(FigureProperty, value);
            }
        }

        /// <summary>
        /// Figures the property changed callback.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void FigurePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            //(dependencyObject as SvgImage).BUTTON_ICO_PATH_DATA.Figures = (PathFigureCollection)dependencyPropertyChangedEventArgs.NewValue;
        }

        #endregion

        #region Fill

        /// <summary>
        /// The fill property
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(SvgImage), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill
        {
            get
            {
                return (Brush)GetValue(FillProperty);
            }

            set
            {
                SetValue(FillProperty, value);
            }
        }
        /// <summary>
        /// Fills the property changed callback.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void FillPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            //(dependencyObject as SvgImage).BUTTON_ICO_PATH.Fill = (Brush)dependencyPropertyChangedEventArgs.NewValue;
        }
        #endregion

        #region ViewBoxWidth

        /// <summary>
        /// The view box width property
        /// </summary>
        public static readonly DependencyProperty ViewBoxWidthProperty = DependencyProperty.Register("ViewBoxWidth", typeof(double), typeof(SvgImage), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the width of the view box.
        /// </summary>
        /// <value>
        /// The width of the view box.
        /// </value>
        public double ViewBoxWidth
        {
            get
            {
                return (double)GetValue(ViewBoxWidthProperty);
            }

            set
            {
                SetValue(ViewBoxWidthProperty, value);
            }
        }
        /// <summary>
        /// Views the box width property changed callback.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="dependencyPropertyChangedEventArgs">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void ViewBoxWidthPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as SvgImage).VIEWBOX.Width = (double)dependencyPropertyChangedEventArgs.NewValue;
        }
        #endregion
    }
}
