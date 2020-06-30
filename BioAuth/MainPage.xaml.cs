using System.ComponentModel;
using Xamarin.Forms;

namespace BioAuth
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, IBioAuthCompleted
    {
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<MainPage, string>(this, "BioAuth", async (senderMsg, arg) =>
            {
                // Do something whenever the "Hi" message is received
                await DisplayAlert("Message received", "arg=" + arg, "OK");
            });
        }

        public async void OnCompleted(BioAuthStatus status)
        {
            await DisplayAlert("Message received", status.ToString(), "OK");
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            DependencyService.Get<IBiometricProvider>().Authenticate(this);
        }
    }
}
