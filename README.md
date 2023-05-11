# AndroidCO2GAZ

Dernière version : 

Internationalisation de l'application.

Cette application Xamarin permet aux utilisateurs de traduire facilement changer la langue entre français et l'anglais en cliquant 
sur un bouton. 
L'application utilise le fichier strings.xml pour stocker les valeurs traduites.

Fonctionnalité : 

  -Traduit l'application entre Français et Anglais.
  -Traduction stocké dans le fichier values/strings.xml et /values-fr/string.xml

Requit : 

  -Visual studio avec le framework Xamarin.

Installation : 

  -Cloner le repertoire git : https://github.com/maxencemazeau/AndroidCO2GAZ.git
  -Ouvrer le MQTT.sln dans Visual Studio.

Usage :

  -Lancer l'application.
  -Changer entre la langue Français et Anglais en cliquant sur le bouton dans la connexion.
  -Dans les options, il est possible de re changer de langue si besoin.
  
Documentation : 

  Exemple du strings.xml : 
    <resources> 
      <string name="app_name">MQTT</string> 
      <string name="action_settings">Settings</string> 
    <resources>
      
  Lier le strings.xml avec le XAML et le C#
  -Définition dans le fichier XAML : 
    Exemple pour le texte qualité de l'air
    android:text="@string/qualite"
  -Dans le C# :
    loginTitleTextView.Text = GetString(Resource.String.login_title); 
