/* Copyright (C) 2023 Maxence MAZEAU
 * All rights reserved.
 *
 * Projet Qualite de l'air
 * Ecole du Web
 * Projet technologique (c)2023
 *  

    Historique des versions
           Version    Date       Auteur       Description
           1.1        24/02/23  Maxence     Première version
           1.2        23/04/23  Maxence     Deuxième version
           1.3       30/04/23   Maxence     Troisième version
   
 * */

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MQTT
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EditText usernameEditText, passwordEditText;
        private Button loginButton;
        private Switch languageSwitch;
        private TextView loginTitleTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            
            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            languageSwitch = FindViewById<Switch>(Resource.Id.languageSwitch);
            loginTitleTextView = FindViewById<TextView>(Resource.Id.loginTitleTextView);


            loginButton.Click += OnLoginButtonClick;
            languageSwitch.CheckedChange += OnLanguageSwitchCheckedChange;
        }

        private async void OnLoginButtonClick(object sender, EventArgs e)
        {
            string username = usernameEditText.Text;
            string password = passwordEditText.Text;

            try
            {
                var api = new ApiCall();
                var user = await api.LoginAsync(username, password);

                //Redirige au dashboard
                Intent intent = new Intent(this, typeof(Dashboard));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void OnLanguageSwitchCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var lang = e.IsChecked ? "en" : "fr";
            ChangeLanguage(lang);
        }

        private void ChangeLanguage(string lang)
        {
            var locale = new Java.Util.Locale(lang);
            Java.Util.Locale.Default = locale;

            var config = new Android.Content.Res.Configuration { Locale = locale };
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);

            // Update UI elements with new language strings
            UpdateUI();
        }

        private void UpdateUI()
        {
            loginTitleTextView.Text = GetString(Resource.String.login_title);
            usernameEditText.Hint = GetString(Resource.String.username_hint);
            passwordEditText.Hint = GetString(Resource.String.password_hint);
            loginButton.Text = GetString(Resource.String.login_button);
            languageSwitch.TextOn = GetString(Resource.String.english_button_label);
            languageSwitch.TextOff = GetString(Resource.String.french_button_label);
        }
    }
}