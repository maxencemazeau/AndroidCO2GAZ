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
           2          05/05/2023 Maxence    Traduction de Anglais / Français de l'application
   
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
        private Button loginButton, languageButton;
        private Switch languageSwitch;
        private TextView loginTitleTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            
            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            languageButton = FindViewById<Button>(Resource.Id.languageButton);
            loginTitleTextView = FindViewById<TextView>(Resource.Id.loginTitleTextView);


            loginButton.Click += OnLoginButtonClick;
            languageButton.Click += OnLanguageButtonClick;
        }

        private void OnLanguageButtonClick(object sender, EventArgs e)
        {
            //Permet de comparer la langue actuel pour changer adéquatement 
            string currentLanguage = languageButton.Text == GetString(Resource.String.english_button_label) ? "en" : "fr";
            string newLanguage = currentLanguage == "en" ? "fr" : "en";
            ChangeLanguage(newLanguage);
        }

        private void ChangeLanguage(string lang)
        {
            // Crée un nouvel objet Java.Util.Locale avec la langue souhaitée (lang)
            var locale = new Java.Util.Locale(lang);
            Java.Util.Locale.Default = locale;

            // Crée une nouvelle configuration en définissant la langue locale avec la nouvelle langue
            var config = new Android.Content.Res.Configuration { Locale = locale };
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);

            // Update l'ui avec la nouvelle langue
            UpdateUI();
        }

        private void UpdateUI()
        {
            //Nom des éléments UI avec les valeurs strings.XML
            loginTitleTextView.Text = GetString(Resource.String.login_title);
            usernameEditText.Hint = GetString(Resource.String.username_hint);
            passwordEditText.Hint = GetString(Resource.String.password_hint);
            loginButton.Text = GetString(Resource.String.login_button);
            languageButton.Text = GetString(Resource.String.english_button_label) == languageButton.Text ? GetString(Resource.String.french_button_label) : GetString(Resource.String.english_button_label);
        }

        private async void OnLoginButtonClick(object sender, EventArgs e)
        {
            string username = usernameEditText.Text;
            string password = passwordEditText.Text;

            try
            {
                //var api = new ApiCall();
                //var user = await api.LoginAsync(username, password);
                if (username == "login1" && password == "pass1")
                {
                    string currentLanguage = languageButton.Text == GetString(Resource.String.english_button_label) ? "en" : "fr";


                    //Redirige au dashboard et envoie la langue
                    Intent intent = new Intent(this, typeof(Dashboard));
                    intent.PutExtra("CurrentLanguage", currentLanguage);
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }
    }
}