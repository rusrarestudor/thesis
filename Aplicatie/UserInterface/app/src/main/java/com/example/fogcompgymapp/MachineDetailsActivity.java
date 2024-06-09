package com.example.fogcompgymapp;

import android.os.Bundle;
import android.widget.TextView;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MachineDetailsActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_machine_details);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
        TextView textViewMachineName = findViewById(R.id.textViewMachineName);
        TextView textViewMachineDescription = findViewById(R.id.textViewMachineDescription);
        TextView textViewMachineUsage = findViewById(R.id.textViewMachineUsage);

        // Assume data is passed via intent
        String machineName = getIntent().getStringExtra("machineName");
        String machineDescription = getIntent().getStringExtra("machineDescription");
        String machineUsage = getIntent().getStringExtra("machineUsage");

        textViewMachineName.setText(machineName);
        textViewMachineDescription.setText(machineDescription);
        textViewMachineUsage.setText(machineUsage);
    }
}