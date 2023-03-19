using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BluescreenSimulator.Views;

namespace BluescreenSimulator.Controls
{
    /// <summary>
    /// Interaction logic for ColorPickerForm.xaml
    /// </summary>
    public partial class ColorPickerForm : UserControl
    {
        public ColorPickerForm()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(ColorPickerForm), new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty IsWin10BgProperty = DependencyProperty.Register(
            "IsWin10Bg", typeof(bool), typeof(ColorPickerForm), new FrameworkPropertyMetadata(false));

        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public bool IsWin10Bg
        {
            get { return (bool)GetValue(IsWin10BgProperty); }
            set { SetValue(IsWin10BgProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new ColorChooserWindow(Color, IsWin10Bg)
            {
                Owner = Window.GetWindow(this)
            };
            window.ActionComplete += (o, args) => Dispatcher.Invoke(() => Color = args.Color);
            window.ShowDialog();
        }
    }
}
