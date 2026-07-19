package ec.edu.ws_eurekabank_climov.controlador;

import android.util.Log;
import android.widget.Toast;

import java.io.IOException;

import ec.edu.ws_eurekabank_climov.modelo.EurekabankService;
import ec.edu.ws_eurekabank_climov.modelo.LoginResponse;
import ec.edu.ws_eurekabank_climov.vista.MainActivity;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


public class LoginControlador {
    private EurekabankService apiService;

    public LoginControlador() {
        apiService = RetrofitClient.getClient().create(EurekabankService.class);
    }

    public void login(String usuario, String password, ConfigCallback<String> callback) {
        Call<ResponseBody> call = apiService.login(usuario, password);
        call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response.isSuccessful()) {
                    try {
                        // Leer la respuesta como texto
                        String serverResponse = response.body() != null ? response.body().string() : "Respuesta vacía del servidor";
                        callback.onSuccess(serverResponse.trim()); // Trim para eliminar espacios o saltos de línea
                    } catch (IOException e) {
                        callback.onError("Error al leer la respuesta del servidor");
                    }
                } else {
                    try {
                        String errorBody = response.errorBody() != null ? response.errorBody().string() : "Error desconocido";
                        callback.onError("Error en el servidor: " + errorBody);
                    } catch (IOException e) {
                        callback.onError("Error al procesar el error del servidor");
                    }
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callback.onError("Error de conexión: " + t.getMessage());
            }
        });
    }

}