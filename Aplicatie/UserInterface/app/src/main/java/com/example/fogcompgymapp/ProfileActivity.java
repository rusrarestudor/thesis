package com.example.fogcompgymapp;

import android.os.Bundle;
import android.view.View;
import android.widget.EditText;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class ProfileActivity extends AppCompatActivity {
    private EditText editTextName, editTextAge, editTextWeight;
    private MQTTClient mqttClient;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_profile);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
        editTextName = findViewById(R.id.editTextName);
        editTextAge = findViewById(R.id.editTextAge);
        editTextWeight = findViewById(R.id.editTextWeight);

        mqttClient = new MQTTClient(this, "tcp://<your_raspberry_pi_ip>:1883", "AndroidClient");
        mqttClient.connect("username", "password");

        findViewById(R.id.buttonSaveProfile).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                saveProfile();
            }
        });
    }

    private void saveProfile() {
        String name = editTextName.getText().toString();
        String age = editTextAge.getText().toString();
        String weight = editTextWeight.getText().toString();

        // Publish profile data to MQTT
        String profileData = "{\"name\":\"" + name + "\",\"age\":\"" + age + "\",\"weight\":\"" + weight + "\"}";
        mqttClient.publishMessage("gym/profile", profileData);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        mqttClient.disconnect();
    }
}