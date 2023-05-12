# AndroidCO2GAZ

## Dernière version : 

Internationalisation de l'application.

Cette application Xamarin permet aux utilisateurs de traduire facilement changer la langue entre français et l'anglais en cliquant 
sur un bouton. 
L'application utilise le fichier strings.xml pour stocker les valeurs traduites.

## Fonctionnalité : 

  -Traduit l'application entre Français et Anglais.
  -Traduction stocké dans le fichier values/strings.xml et /values-fr/string.xml

## Requit : 

  -Visual studio avec le framework Xamarin.

## Installation : 

  -Cloner le repertoire git : https://github.com/maxencemazeau/AndroidCO2GAZ.git
  -Ouvrer le MQTT.sln dans Visual Studio.

## Usage :

  -Lancer l'application.
  -Changer entre la langue Français et Anglais en cliquant sur le bouton dans la connexion.
  -Dans les options, il est possible de re changer de langue si besoin.
  
## Documentation : 

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
      
## Version précédente :
      
    ##Connexion au MQTT : 
        -IP : 172.16.5.100
        -Mot de passe : mams
        -Lire le topic sur le MQTT : mosquitto_sub -h localhost -t \topic -d
      
      Publish sur le broker MQTT : 
        -ESP : 
          //Creer un topic 
          Adafruit_MQTT_Publish topic_publish_GAZ = Adafruit_MQTT_Publish(&mqtt, "GAZ"); 
          //Publier un topic 
          topic_publish_GAZ.publish(buffer2); 
      
      -Android : 
        -Connexion au Mqtt (« subscriber ») 
          Librarie pour le MQTT : using MQTTnet; 
          using MQTTnet.Client.Options; 
          using MQTTnet.Client; 
      
          // Connexion au MQTT  
          var options = new MqttClientOptionsBuilder() 
          .WithTcpServer("172.16.5.100") 
          .WithCredentials("mams", "mams") 
          .Build(); 
      
           // Connection au broker MQTT 
          await mqttClient.ConnectAsync(options); 
          isConnected = true; 
          
          // Subscribe au topic CO2 
          var subscribeResult = await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("CO2").Build(), 
          new TopicFilterBuilder().WithTopic("GAZ").Build());
      
    ## Connecter les senseurs : 
        
      GAZ : 
      
         Librairie : 
            #include <MQUnifiedsensor.h> 
            #define GAS_SENSOR 36 

            //Publish
            Adafruit_MQTT_Publish topic_publish_GAZ = Adafruit_MQTT_Publish(&mqtt, "GAZ"); 
      
            //Lire les valeurs
            int gas_ppm = analogRead(GAS_SENSOR); 
            sprintf(buffer2, "%d", gas_ppm); 
            topic_publish_GAZ.publish(buffer2); 
      
      CO2 : 
      
        Librarie : 
          #include <ccs811.h> 
      
          //Publish 
          Adafruit_MQTT_Publish topic_publish_GAZ = Adafruit_MQTT_Publish(&mqtt, "GAZ"); 
          
          //Lire les valeurs
          uint16_t eco2, etvoc, errstat, raw;
          ccs811.read(&eco2, &etvoc, &errstat, &raw);
 
          if ( errstat == CCS811_ERRSTAT_OK )
            {
              val1 = eco2;
              val2 = etvoc;
              sprintf(buffer1, "%f", val1);
              topic_publish_CO2.publish(buffer1);
            }
