using Microsoft.Maui.Controls.Shapes;
using Point = Microsoft.Maui.Graphics.Point;
using SColor = Microsoft.Maui.Graphics.Color;

namespace DSoft.Maui.Controls
{
    public class BubbleView : ContentView
    {
        #region Fields

        private readonly Border _background;
        private readonly Label _label;

        #endregion

        #region BubbleColor

        public static readonly BindableProperty BubbleColorProperty = BindableProperty.Create(nameof(BubbleColor), typeof(SColor), typeof(BubbleView), Colors.Red, propertyChanged: RedrawCanvas);

        public SColor BubbleColor
        {
            get => (SColor)GetValue(BubbleColorProperty);
            set => SetValue(BubbleColorProperty, value);
        }

        #endregion

        #region HasShadow

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(BubbleView), true, propertyChanged: RedrawCanvas);

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        #endregion

        #region BorderColor

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(SColor), typeof(BubbleView), Colors.Transparent, propertyChanged: RedrawCanvas);

        public SColor BorderColor
        {
            get => (SColor)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        #endregion

        #region TextColor

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(SColor), typeof(BubbleView), Colors.White, propertyChanged: RedrawCanvas);

        public SColor TextColor
        {
            get => (SColor)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        #endregion

        #region Text

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(BubbleView), "0", propertyChanged: RedrawCanvas);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region Constructors
        
        public BubbleView()
        {
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
            Padding = 0;
            
            _background = new Border()
            {
                BackgroundColor = Colors.Red,
            };


            _label = new Label()
            {
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            _background.Content = _label;

            this.Content = _background;
        }

        
        #endregion

        #region Methods
        
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (!(height < 0))
            {
                _background.StrokeShape = new RoundRectangle()
                {
                    CornerRadius = (float)height / 2,
                };
                
                _background.Padding = new Thickness(0, 0, 0, 0);

                _background.WidthRequest = _label.DesiredSize.Width + 14;

            }


        }

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            var self = bindable as BubbleView;


            self?.UpdateContent();

        }

        private void UpdateContent()
        {
            _background.BackgroundColor = BubbleColor;
            _label.Text = Text;
            _label.TextColor = TextColor;
            
            _background.Stroke = BorderColor;
            
            if (HasShadow)
            {
                _background.Shadow = new Shadow()
                {
                    Brush = Colors.Black,
                    Offset = new Point(2,2),
                    Radius=4,
                    Opacity=0.8f,
                };
            }
            else
            {
                _background.Shadow = null;
            }

        }
        
        #endregion
    }
}
