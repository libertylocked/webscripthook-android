using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;

namespace webscripthook_android
{
    [Activity(Label = "GTAV Mobile Dashboard", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            Window.RequestFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var txtIpAddr = FindViewById<EditText>(Resource.Id.editTextIpAddr);
            var txtPort = FindViewById<EditText>(Resource.Id.editTextPort);
            var btnConnect = FindViewById<Button>(Resource.Id.buttonConnect);

            // Load IP address from pref
            txtIpAddr.Text = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext).GetString("ServerIP", "");
            txtPort.Text = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext).GetInt("ServerPort", 25555).ToString();
            

            btnConnect.Click += onButtonConnect_Pressed;
        }

        private void onButtonConnect_Pressed(object sender, EventArgs e)
        {
            var ipAddr = FindViewById<EditText>(Resource.Id.editTextIpAddr).Text;
            var port = 25555; 
            // Verify port
            if (!int.TryParse(FindViewById<EditText>(Resource.Id.editTextPort).Text, out port))
            {
                Toast.MakeText(ApplicationContext, "Port is not a number!", ToastLength.Short).Show();
                return;
            }

            // Check plugin status
            if (!requestPluginStatus(ipAddr, port)) return;

            // Save server address if checked
            if (FindViewById<CheckBox>(Resource.Id.checkBoxSave).Checked)
            {
                saveServerAddress(ipAddr, port);
            }

            // Launch webview!
            Intent webIntent = new Intent(this, typeof(WebActivity));
            webIntent.PutExtra("Address", "http://" + ipAddr + ":" + port);
            StartActivity(webIntent);
            Finish();
        }

        private void saveServerAddress(string ipAddr, int port)
        {
            var editor = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext).Edit();
            editor.PutString("ServerIP", ipAddr);
            editor.PutInt("ServerPort", port);
            editor.Commit();
        }

        private bool requestPluginStatus(string ipAddr, int port)
        {
            bool connected = false;
            try
            {
                var webClient = new System.Net.WebClient();
                var response = webClient.DownloadString("http://" + ipAddr + ":" + port + "/connected");
                connected = response.ToLower() == "true";
                if (!connected)
                {
                    Toast.MakeText(ApplicationContext, "Plugin not connected!\nIs the game running?", ToastLength.Long).Show();
                }
            }
            catch
            {
                Toast.MakeText(ApplicationContext, "Unable to connect to server :(", ToastLength.Short).Show();
            }
            return connected;
        }
    }
}

