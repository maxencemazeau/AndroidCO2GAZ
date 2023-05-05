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
           1.3        30/04/23  Maxence     Troisième version
   
 * */

using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using Android.Widget;
using MQTTnet.Client.Subscribing;
using System.Text;
using System.Collections.Generic;
using Android.Content;
using MQTT;
using System.Threading.Tasks;

namespace MQTT
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class Options : Activity
    {
      

        private EditText topicEditText;

        private Button retour;

        private Button subscribeButton;

        private Button languageButton;

        private Button topicButton;
        private TextView moyenEditText;
        private TextView fortEditText;

        private TextView mdpTextView;

        private Button brokerButton;
        private TextView ipEditText;

        private IMqttClient mqttClient;
        private bool isConnected = false;

        private MQTTService mqttService;

        private string currentLanguage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.content_main);

            

            //Elements XAML
            topicButton = FindViewById<Button>(Resource.Id.buttonDuTopic);
            mdpTextView = FindViewById<TextView>(Resource.Id.mdpbroker);

            topicEditText = FindViewById<EditText>(Resource.Id.topic);

            ipEditText = FindViewById<TextView>(Resource.Id.ipbroker);

            moyenEditText = FindViewById<TextView>(Resource.Id.moyen);

            fortEditText = FindViewById<TextView>(Resource.Id.fort);

            retour = FindViewById<Button>(Resource.Id.retour);

            languageButton = FindViewById<Button>(Resource.Id.languageButton);

            currentLanguage = Intent.GetStringExtra("CurrentLanguage");

            if( currentLanguage == "en")
            {
                languageButton.Text = GetString(Resource.String.english_button_label);
            } else
            {
                languageButton.Text = GetString(Resource.String.french_button_label);
            }

            // Creer le client MQTT
            mqttClient = new MqttFactory().CreateMqttClient();

            topicButton.Click += OnTopicButtonClick;
            languageButton.Click += OnLanguageButtonClick;

            retour.Click += OnRetourButtonClick;

            var mqttService = new MQTTService();
            

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
            mdpTextView.Hint = GetString(Resource.String.motDePasse);

            topicEditText.Hint = GetString(Resource.String.topic);

            ipEditText.Hint = GetString(Resource.String.adresseIp);

            moyenEditText.Hint = GetString(Resource.String.moyenne);

            fortEditText.Hint = GetString(Resource.String.haute);

            topicButton.Text = GetString(Resource.String.abonner);

            retour.Text = GetString(Resource.String.retour);
            languageButton.Text = GetString(Resource.String.english_button_label) == languageButton.Text ? GetString(Resource.String.french_button_label) : GetString(Resource.String.english_button_label);
        }

        private void OnLanguageButtonClick(object sender, EventArgs e)
        {
            string currentLanguage = languageButton.Text == GetString(Resource.String.english_button_label) ? "en" : "fr";
            string newLanguage = currentLanguage == "en" ? "fr" : "en";
            ChangeLanguage(newLanguage);
            this.currentLanguage = newLanguage;
        }

        private void OnRetourButtonClick(object sender, EventArgs e)
        {
  
            Intent intent = new Intent(this, typeof(Dashboard));
            intent.PutExtra("CurrentLanguage", currentLanguage);
            StartActivity(intent);
        }

        private async void OnTopicButtonClick(object sender, EventArgs e)
        {
            string topic = topicEditText.Text;
            string brokerIp = ipEditText.Text;
            string mdp = mdpTextView.Text;
            string mid = moyenEditText.Text;
            string high = fortEditText.Text;

            if (!isConnected)
            {
                //Connection au broker
                var options = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerIp)
                .WithCredentials(mdp, mdp)
                .Build();

                
                await mqttClient.ConnectAsync(options);

                isConnected = true;

                //Subscribe au topic
                var subscribeResult = await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());

                //Gestionnaire d'événements pour la réception de messages
                mqttClient.UseApplicationMessageReceivedHandler(async e =>
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                   
                
                    AppData.MQTTService.MessageTopic = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    AppData.MQTTService.TitreTopic = e.ApplicationMessage.Topic;
                    AppData.MQTTService.Moyen = mid;
                    AppData.MQTTService.Fort = high;
                    AppData.MQTTService.Ip = brokerIp;
                    AppData.MQTTService.Mdp = mdp;

                });
            }
            else
            {
                //Deconnecte du client MQTT
                await mqttClient.DisconnectAsync();

                isConnected = false;


               
            }
        }

        
    }
}