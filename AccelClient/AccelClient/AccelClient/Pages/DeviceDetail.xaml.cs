using AccelClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AccelClient
{


    public partial class DeviceDetail : ContentPage
    {
        public DeviceDetail() { }

        public DeviceDetail(DeviceDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;

            imageDevice.Source = ImageSource.FromResource($"AccelClient.Images.{viewModel.TypeOfDevice}.png");
        }
    }
}