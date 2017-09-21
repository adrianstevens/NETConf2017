using System;
using Xamarin.Forms;
using XFDraw.Views;

#if __ANDROID__
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Support.Design.Widget;
#endif

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
#if __ANDROID__
            var actionButton = new FloatingActionButton(Forms.Context);

            actionButton.SetImageResource(XFDraw.Droid.Resource.Drawable.trash);
            actionButton.Click += (s, e) => OnClearClicked();

            var actionButtonFrame = new FrameLayout(Forms.Context);
            actionButtonFrame.SetClipToPadding(false);
            actionButtonFrame.SetPadding(0, 0, 50, 50);
            actionButtonFrame.AddView(actionButton);

            var actionButtonFrameView = actionButtonFrame.ToView();
            actionButtonFrameView.HorizontalOptions = LayoutOptions.End;
            actionButtonFrameView.VerticalOptions = LayoutOptions.End;


            mainLayout.Children.Add(actionButtonFrameView, 0, 1);
#else
            clearCommand = new Command(OnClearClicked, () => { return IsCanvasDirty; });

            var trash = new ToolbarItem()
            {
                Text = "Clear",
                Icon = "trash.png",
                Command = clearCommand
            };

            ToolbarItems.Add(trash);
#endif
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