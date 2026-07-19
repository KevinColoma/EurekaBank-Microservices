package ec.edu.ws_eurekabank_climov.vista;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.DividerItemDecoration;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import ec.edu.ws_eurekabank_climov.R;
import ec.edu.ws_eurekabank_climov.controlador.MovimientosAdapter;
import ec.edu.ws_eurekabank_climov.controlador.RetrofitClient;
import ec.edu.ws_eurekabank_climov.modelo.EurekabankService;
import ec.edu.ws_eurekabank_climov.modelo.Movimiento;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MovimientoActivity extends AppCompatActivity {

    private EurekabankService eurekaBankService;
    private RecyclerView rvMovimientos;
    private MovimientosAdapter adapter;
    private List<Movimiento> movimientos;
    private EditText txtCuenta;
    private Button btnConsultar;
    private Button btnRegresar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_movimiento);

        inizializar();
        eurekaBankService = RetrofitClient.getClient().create(EurekabankService.class);
        rvMovimientos = findViewById(R.id.rvMovimientos);
        rvMovimientos.setLayoutManager(new LinearLayoutManager(this));

        // Agregar divisores entre elementos
        DividerItemDecoration divider = new DividerItemDecoration(this, DividerItemDecoration.VERTICAL);
        rvMovimientos.addItemDecoration(divider);

        btnConsultar.setOnClickListener(v->{
            String cuenta = txtCuenta.getText().toString().trim();
            obtenerMovimientos(cuenta);
        });
        btnRegresar.setOnClickListener(v->{
            regresar();
        });

    }

    private void obtenerMovimientos(String cuenta) {
        Call<List<Movimiento>> call = eurekaBankService.obtenerMovimientos(cuenta);
        call.enqueue(new Callback<List<Movimiento>>() {
            @Override
            public void onResponse(Call<List<Movimiento>> call, Response<List<Movimiento>> response) {
                if (response.isSuccessful()) {
                    movimientos = response.body();
                    adapter = new MovimientosAdapter(movimientos);
                    rvMovimientos.setAdapter(adapter);
                } else {
                    // Manejar error de respuesta
                    limpiarTabla();
                    String errorMessage = "Error al obtener los movimientos";
                    if (response.errorBody() != null) {
                        try {
                            JSONObject errorJson = new JSONObject(response.errorBody().string());
                            if (errorJson.has("error")) {
                                errorMessage = errorJson.getString("error");
                            }
                        } catch (IOException | JSONException e) {
                            e.printStackTrace();
                        }
                    }
                    mostrarMensajeError(errorMessage);
                }
            }

            @Override
            public void onFailure(Call<List<Movimiento>> call, Throwable t) {
                // Manejar error de conexión
                mostrarMensajeError("Error de conexión: " + t.getMessage());
            }
        });
    }

    private void limpiarTabla() {
        movimientos = new ArrayList<>(); // Asignar una lista vacía
        if (adapter != null) {
            adapter = new MovimientosAdapter(movimientos);
            rvMovimientos.setAdapter(adapter);
        }
    }

    private void inizializar(){
        txtCuenta = findViewById(R.id.txtCuenta);
        btnConsultar = findViewById(R.id.btnConsultar);
        btnRegresar = findViewById(R.id.btnRegresar);
    }
    private void mostrarMensajeError(String mensaje) {
        Toast.makeText(this, mensaje, Toast.LENGTH_SHORT).show();
    }
    public void regresar(){
        startActivity(new Intent(MovimientoActivity.this, MenuActivity.class));
        finish();
    }
}