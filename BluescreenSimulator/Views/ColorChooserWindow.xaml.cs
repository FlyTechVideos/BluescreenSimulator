using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using Color = System.Windows.Media.Color;

namespace BluescreenSimulator.Views
{
    /// <summary>
    /// Wow a color chooser 
    /// </summary>
    public partial class ColorChooserWindow : Window
    {
        private readonly ColorWindowData _data;
        public ColorChooserWindow(Color? color = null)
        {
            _data = new ColorWindowData();
            if (color != null)
            {
                var drawingColor =
                    System.Drawing.Color.FromArgb(color.Value.A, color.Value.R, color.Value.G, color.Value.B);
                _data.SetValueFromDrawingColor(drawingColor);
            }
            DataContext = _data;
            InitializeComponent();
        }
        private class ColorWindowData : PropertyChangedObject
        {
            public ColorWindowData()
            {

            }
            private double _hue;

            public double Hue
            {
                get => _hue;
                set { _hue = value; OnPropertyChanged(); UpdateColors(); }
            }
            private double _saturation = 90;

            public double Saturation
            {
                get => _saturation;
                set { _saturation = value; OnPropertyChanged(); UpdateColors(); }
            }
            private double _lightness = 60;

            public double Lightness
            {
                get => _lightness;
                set { _lightness = value; OnPropertyChanged(); UpdateColors(); }
            }
            private double _opacity = 1;

            public double Opacity
            {
                get => _opacity;
                set { _opacity = value; OnPropertyChanged(); UpdateColors(); }
            }

            private void UpdateColors()
            {
                OnPropertyChanged(nameof(ResultColor));
                OnPropertyChanged(nameof(ResultTextColor));
                OnPropertyChanged(nameof(FullSaturationColor));
            }

            private Color ChangeOpacity(Color c)
            {
                if (Opacity == 0) Opacity += double.Epsilon;
                c.A = (byte)Math.Round(Opacity * 255);
                return c;
            }
            public Color ResultColor => ChangeOpacity(HslToRgb(_hue / 100, _saturation / 100, _lightness / 100));

            public string ResultTextColor
            {
                get => ResultColor.ToString();
                set
                {
                    try
                    {
                        var c = ColorTranslator.FromHtml(value);
                        SetValueFromDrawingColor(c);
                    }
                    catch (Exception)
                    {
                        // whatever
                    }
                }
            }

            public void SetValueFromDrawingColor(System.Drawing.Color c)
            {
                _hue = c.GetHue() / 3.6;
                _lightness = c.GetBrightness() * 100;
                _saturation = c.GetSaturation() * 100;
                _opacity = c.A / (float)255;
                OnPropertyChanged(nameof(Hue));
                OnPropertyChanged(nameof(Lightness));
                OnPropertyChanged(nameof(Saturation));
                OnPropertyChanged(nameof(Opacity));
                OnPropertyChanged(nameof(ResultColor));
                OnPropertyChanged(nameof(FullSaturationColor));
            }

            public Color FullSaturationColor => ChangeOpacity(HslToRgb(_hue / 100, 1, 0.5));


            // Given H,S,L in range of 0-1
            // Returns a Color (RGB struct) in range of 0-255
            public static Color HslToRgb(double h, double sl, double l)
            {
                double v;
                double r, g, b;

                r = l;   // default to gray
                g = l;
                b = l;
                v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
                if (v > 0)
                {
                    double m;
                    double sv;
                    int sextant;
                    double fract, vsf, mid1, mid2;

                    m = l + l - v;
                    sv = (v - m) / v;
                    h *= 6.0;
                    sextant = (int)h;
                    fract = h - sextant;
                    vsf = v * sv * fract;
                    mid1 = m + vsf;
                    mid2 = v - vsf;
                    switch (sextant)
                    {
                        case 0:
                            r = v;
                            g = mid1;
                            b = m;
                            break;
                        case 1:
                            r = mid2;
                            g = v;
                            b = m;
                            break;
                        case 2:
                            r = m;
                            g = v;
                            b = mid1;
                            break;
                        case 3:
                            r = m;
                            g = mid2;
                            b = v;
                            break;
                        case 4:
                            r = mid1;
                            g = m;
                            b = v;
                            break;
                        case 5:
                            r = v;
                            g = m;
                            b = mid2;
                            break;
                        case 6:
                            goto case 0;
                    }
                }
                unchecked
                {
                    return Color.FromRgb((byte)(r * 255.0f), (byte)(g * 255.0f), (byte)(b * 255.0f));
                }
            }

            /// <summary>
            /// Clamp a value to 0-255
            /// </summary>
            private static byte Clamp(int i)
            {
                if (i < 0) return 0;
                if (i > 255) return 255;
                return (byte)i;
            }
        }

        public event EventHandler<ColorRequestEventArgs> ActionComplete;
        public event EventHandler ActionCanceled;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ActionComplete?.Invoke(this, new ColorRequestEventArgs(_data.ResultColor));
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ActionCanceled?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void ColorTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }
        }
    }
    public class ColorRequestEventArgs : EventArgs
    {
        public Color Color { get; }

        public ColorRequestEventArgs(Color c) => Color = c;
    }
}
