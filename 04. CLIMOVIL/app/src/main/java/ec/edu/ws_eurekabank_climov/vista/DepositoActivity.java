package ec.edu.ws_eurekabank_climov.vista;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Spinner;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import ec.edu.ws_eurekabank_climov.R;
import ec.edu.ws_eurekabank_climov.controlador.ConfigCallback;
import ec.edu.ws_eurekabank_climov.controlador.EurekabankControlador;

public class DepositoActivity extends AppCompatActivity {
    private EditText txtCuenta;
    private EditText txtImporte;
    private EditText txtCuentaOrigen;
    private EditText txtCuentaDestino;
    private Button btnProcesar;
    private Button btnRegresar;
    private ProgressBar progressBar;
    private Spinner spinnerOperacion;
    private EurekabankControlador eurekabankControlador;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_deposito);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
        inicializar();

        btnRegresar.setOnClickListener(v -> regresar());
        btnProcesar.setOnClickListener(v -> procesarOperacion());

        configurarSpinner();
    }

    public void inicializar() {
        eurekabankControlador = new EurekabankControlador();
        btnProcesar = findViewById(R.id.btnProcesar);
        btnRegresar = findViewById(R.id.btnRegresar);
        txtCuenta = findViewById(R.id.txtCuenta);
        txtImporte = findViewById(R.id.txtImporte);
        txtCuentaOrigen = findViewById(R.id.txtCuentaOrigen);
        txtCuentaDestino = findViewById(R.id.txtCuentaDestino);
        progressBar = findViewById(R.id.progressBar);
        spinnerOperacion = findViewById(R.id.spinnerOperacion);
    }

    private void configurarSpinner() {
        spinnerOperacion.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                String operacion = parent.getItemAtPosition(position).toString();

                // Mostrar/ocultar campos según la operación seleccionada
                if (operacion.equals("Depósito") || operacion.equals("Retiro")) {
                    txtCuenta.setVisibility(View.VISIBLE);
                    txtImporte.setVisibility(View.VISIBLE);
                    txtCuentaOrigen.setVisibility(View.GONE);
                    txtCuentaDestino.setVisibility(View.GONE);
                } else if (operacion.equals("Transferencia")) {
                    txtCuenta.setVisibility(View.GONE);
                    txtImporte.setVisibility(View.VISIBLE);
                    txtCuentaOrigen.setVisibility(View.VISIBLE);
                    txtCuentaDestino.setVisibility(View.VISIBLE);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                // No hacer nada
            }
        });
    }

    public void regresar() {
        startActivity(new Intent(DepositoActivity.this, MenuActivity.class));
        finish();
    }

    public void procesarOperacion() {
        String operacion = spinnerOperacion.getSelectedItem().toString();

        if (operacion.equals("Depósito")) {
            procesarDeposito();
        } else if (operacion.equals("Retiro")) {
            procesarRetiro();
        } else if (operacion.equals("Transferencia")) {
            procesarTransferencia();
        }
    }

    private void procesarDeposito() {
        String cuenta = txtCuenta.getText().toString().trim();
        String importeStr = txtImporte.getText().toString().trim();

        if (cuenta.isEmpty() || importeStr.isEmpty()) {
            mostrarMensajeError("Por favor, complete todos los campos");
            return;
        }

        try {
            double importe = Double.parseDouble(importeStr);
            progressBar.setVisibility(View.VISIBLE);
            btnProcesar.setEnabled(false);

            eurekabankControlador.realizarDeposito(cuenta, importe, new ConfigCallback<String>() {
                @Override
                public void onSuccess(String mensaje) {
                    progressBar.setVisibility(View.GONE);
                    btnProcesar.setEnabled(true);
                    mostrarMensajeExito(mensaje);
                    limpiarCampos();
                }

                @Override
                public void onError(String error) {
                    progressBar.setVisibility(View.GONE);
                    btnProcesar.setEnabled(true);
                    mostrarMensajeError(error);
                }
            });
        } catch (NumberFormatException e) {
            mostrarMensajeError("Ingrese un importe válido");
        }
    }

    private void procesarRetiro() {
        String cuenta = txtCuenta.getText().toString().trim();
        String importeStr = txtImporte.getText().toString().trim();

        if (cuenta.isEmpty() || importeStr.isEmpty()) {
            mostrarMensajeError("Por favor, complete todos los campos");
            return;
        }

        try {
            double importe = Double.parseDouble(importeStr);
            progressBar.setVisibility(View.VISIBLE);
            btnProcesar.setEnabled(false);

            eurekabankControlador.realizarRetiro(cuenta, importe, new ConfigCallback<String>() {
                @Override
                public void onSuccess(String mensaje) {
                    progressBar.setVisibility(View.GONE);
                    btnProcesar.setEnabled(true);
                    mostrarMensajeExito(mensaje);
                    limpiarCampos();
                }

                @Override
                public void onError(String error) {
                    progressBar.setVisibility(View.GONE);
                    btnProcesar.setEnabled(true);
                    mostrarMensajeError(error);
                }
            });
        } catch (NumberFormatException e) {
            mostrarMensajeError("Ingrese un importe válido");
        }
    }

    private void procesarTransferencia() {
        String cuentaOrigen = txtCuentaOrigen.getText().toString().trim();
        String cuentaDestino = txtCuentaDestino.getText().toString().trim();
        String importeStr = txtImporte.getText().toString().trim();

        if (cuentaOrigen.isEmpty() || cuentaDestino.isEmpty() || importeStr.isEmpty()) {
            mostrarMensajeError("Por favor, complete todos los campos");
            return;
        }

        try {
            double importe = Double.parseDouble(importeStr);
            progressBar.setVisibility(View.VISIBLE);
            btnProcesar.setEnabled(false);

            eurekabankControlador.realizarTransferencia(cuentaOrigen, cuentaDestino, importe, new ConfigCallback<String>() {
                @Override
                public void onSuccess(String mensaje) {
                    progressBar.setVisibility(View.GONE);
                    btnProcesar.setEnabled(true);
                    mostrarMensajeExito(mensaje);
                    limpiarCampos();
                }

                @Override
                public void onError(String error) {
                    progressBar.setVisibility(View.GONE);
                    btnProcesar.setEnabled(true);
                    mostrarMensajeError(error);
                }
            });
        } catch (NumberFormatException e) {
            mostrarMensajeError("Ingrese un importe válido");
        }
    }

    private void mostrarMensajeExito(String mensaje) {
        new AlertDialog.Builder(this)
                .setTitle("Éxito")
                .setMessage(mensaje)
                .setPositiveButton("Aceptar", null)
                .show();
    }

    private void mostrarMensajeError(String error) {
        new AlertDialog.Builder(this)
                .setTitle("Error")
                .setMessage(error)
                .setPositiveButton("Aceptar", null)
                .show();
    }

    private void limpiarCampos() {
        txtCuenta.setText("");
        txtImporte.setText("");
        txtCuentaOrigen.setText("");
        txtCuentaDestino.setText("");
    }
}
