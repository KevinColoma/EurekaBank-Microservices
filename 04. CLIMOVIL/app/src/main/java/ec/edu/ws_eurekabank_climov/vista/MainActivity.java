package ec.edu.ws_eurekabank_climov.vista;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.regex.Pattern;

import ec.edu.ws_eurekabank_climov.R;
import ec.edu.ws_eurekabank_climov.controlador.ConfigCallback;
import ec.edu.ws_eurekabank_climov.controlador.LoginControlador;
import ec.edu.ws_eurekabank_climov.controlador.RetrofitClient;
import ec.edu.ws_eurekabank_climov.modelo.Config;

public class MainActivity extends AppCompatActivity {
    private EditText editTextUsername;
    private EditText editTextPassword;
    private EditText txtIp;
    private Button btnLogin;
    private ProgressBar progressBar;
    private SharedPreferences sharedPreferences;

    private static final Pattern IP_PATTERN = Pattern.compile(
            "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"
    );

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        // Vincular vistas
        editTextUsername = findViewById(R.id.txtUsername);
        editTextPassword = findViewById(R.id.txtPassword);
        txtIp = findViewById(R.id.txtIp);
        btnLogin = findViewById(R.id.btnLogin);
        progressBar = findViewById(R.id.progressBar);

        sharedPreferences = getSharedPreferences("EurekabankPrefs", MODE_PRIVATE);

        // Cargar IP guardada
        String ipGuardada = sharedPreferences.getString("server_ip", null);
        if (ipGuardada != null) {
            txtIp.setText(ipGuardada);
        } else {
            txtIp.setText("10.9.8.137");
        }

        // Configuración de insets
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        btnLogin.setOnClickListener(v -> {
            String username = editTextUsername.getText().toString().trim();
            String password = editTextPassword.getText().toString().trim();
            String ip = txtIp.getText().toString().trim();

            // Validar IP
            if (!validarIp(ip)) {
                mostrarMensajeError("Ingrese una dirección IP válida (ej: 10.9.8.137)");
                return;
            }

            // Validación de campos
            if (username.isEmpty() || password.isEmpty()) {
                mostrarMensajeError("Por favor, ingrese usuario y contraseña");
                return;
            }

            // Actualizar IP y reiniciar Retrofit
            Config.actualizarIp(ip);
            RetrofitClient.reiniciar();
            sharedPreferences.edit().putString("server_ip", ip).apply();

            // Mostrar ProgressBar
            progressBar.setVisibility(View.VISIBLE);
            btnLogin.setEnabled(false);

            // Llamada al servicio de login
            LoginControlador loginControlador = new LoginControlador();
            loginControlador.login(username, password, new ConfigCallback<String>() {
                @Override
                public void onSuccess(String result) {
                    // Ocultar ProgressBar
                    progressBar.setVisibility(View.GONE);
                    btnLogin.setEnabled(true);

                    // Limpiar campos sensibles
                    limpiarCampos();

                    // Verificar si el login fue exitoso
                    if ("Usuario válido.".equalsIgnoreCase(result)) {
                        mostrarMensajeExito("Bienvenido " + username);

                        // Navegar al menú principal
                        startActivity(new Intent(MainActivity.this, MenuActivity.class));
                        finish();
                    } else {
                        mostrarMensajeError(result); // Mensaje de error devuelto por el servidor
                    }
                }

                @Override
                public void onError(String error) {
                    // Ocultar ProgressBar
                    progressBar.setVisibility(View.GONE);
                    btnLogin.setEnabled(true);

                    // Mostrar mensaje de error
                    mostrarMensajeError(error);
                }
            });
        });

    }

    // Método centralizado para mostrar mensajes de error
    private void mostrarMensajeError(String mensaje) {
        // Puedes personalizar esto para usar Snackbar, un diálogo, etc.
        Toast.makeText(this, "Error: " + mensaje, Toast.LENGTH_SHORT).show();
    }

    // Método centralizado para mostrar mensajes de éxito
    private void mostrarMensajeExito(String mensaje) {
        Toast.makeText(this, mensaje, Toast.LENGTH_SHORT).show();
    }

    // Método para limpiar campos sensibles después del login
    private void limpiarCampos() {
        editTextPassword.setText("");
    }

    private boolean validarIp(String ip) {
        if (ip == null || ip.isEmpty()) {
            return false;
        }
        return IP_PATTERN.matcher(ip).matches();
    }
}