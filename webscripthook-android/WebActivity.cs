using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace webscripthook_android
{
    [Activity(Label = "GTAV WebScriptHook", ScreenOrientation = ScreenOrientation.Sensor)]
    public class WebActivity : Activity
    {
        WebView webView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            Window.RequestFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Web);

            webView = FindViewById<WebView>(Resource.Id.webView1);
            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new WebViewClient()); // stops request going to Web Browser

            if (savedInstanceState == null)
            {
                webView.LoadUrl(Intent.GetStringExtra("Address"));
            }
        }

        public override void OnBackPressed()
        {
            if (webView.CanGoBack())
            {
                webView.GoBack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        protected override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            webView.SaveState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            webView.RestoreState(savedInstanceState);
        }
    }
}