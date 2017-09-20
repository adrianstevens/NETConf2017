using System;
using Xamarin.Forms;
using XFDraw.Views;

namespace XFDraw
{
    public partial class MainPage : ContentPage
    {
        bool IsCanvasDirty
        {
            get { return isCanvasDirty; }
            set
            {
                isCanvasDirty = value;

                if (clearCommand != null)
                    clearCommand.ChangeCanExecute();
            }
        }
        bool isCanvasDirty;

        Command clearCommand;

        ColorPicker colorPicker;

        public MainPage()
        {
            InitializeComponent();

            AddColorPicker();

            AddToolbarItems();

            sketchView.SketchUpdated += OnSketchUpdated;
        }

        void AddColorPicker ()
        {
            if (Device.Idiom == TargetIdiom.Desktop)
                colorPicker = new ColorPickerMouseView();
            else
                colorPicker = new ColorPickerTouchView();

            colorPicker.VerticalOptions = LayoutOptions.Start;
            colorPicker.ColorChanged += ColorPickerColorChanged;

            mainLayout.Children.Add(colorPicker);
        }

        void ColorPickerColorChanged(object sender, EventArgs e)
        {
            sketchView.InkColor = colorPicker.CurrentColor;
        }

        void AddToolbarItems ()
        {
            clearCommand = new Command(OnClearClicked, () => { return IsCanvasDirty; });

            var trash = new ToolbarItem()
            {
                Text = "Clear",
                Icon = "trash.png",
                Command = clearCommand
            };

            ToolbarItems.Add(trash);
        }

        void OnSketchUpdated(object sender, EventArgs e)
        {
            IsCanvasDirty = true;
        }

        void OnClearClicked ()
        {
            sketchView.Clear();
            IsCanvasDirty = false;
        }
    }
}