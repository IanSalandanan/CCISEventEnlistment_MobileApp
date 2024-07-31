using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace IT115L_Oracle_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button logInBtn, signUpBtn, exitBtn;
        private ImageView logo;
        private Functions functions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.lobby);

            logInBtn = FindViewById<Button>(Resource.Id.logInBtn);
            signUpBtn = FindViewById<Button>(Resource.Id.signUpBtn);
            exitBtn = FindViewById<Button>(Resource.Id.exitBtn);
            logo = FindViewById<ImageView>(Resource.Id.lobbyLogo);
       
            logo.SetImageResource(Resource.Drawable.ccisLogo);

            logInBtn.Click += delegate
            {
                functions = new Functions(this, typeof(Login));
                functions.NextActivity();
            };

            signUpBtn.Click += delegate
            {
                functions = new Functions(this, typeof(Register));
                functions.NextActivity();
            };

            exitBtn.Click += delegate
            {
                FinishAffinity();
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}