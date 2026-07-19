package ec.edu.ws_eurekabank_climov.controlador;

import org.json.JSONObject;

import ec.edu.ws_eurekabank_climov.modelo.DepositoResponse;
import ec.edu.ws_eurekabank_climov.modelo.EurekabankService;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class EurekabankControlador {
    private EurekabankService apiService;

    public EurekabankControlador() {
        apiService = RetrofitClient.getClient().create(EurekabankService.class);
    }

    public void realizarDeposito(String cuenta, double importe, ConfigCallback<String> callback) {
        Call<ResponseBody> call = apiService.hacerDeposito(cuenta, importe);
        call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                try {
                    if (response.isSuccessful()) {
                        // Leer el mensaje de respuesta como texto plano
                        String mensaje = response.body() != null ? response.body().string() : "Respuesta vacía del servidor";
                        callback.onSuccess(mensaje.trim()); // Enviar mensaje al callback
                    } else {
                        String errorBody = response.errorBody() != null ? response.errorBody().string() : "Error desconocido";
                        callback.onError("Error del servidor: " + errorBody.trim());
                    }
                } catch (Exception e) {
                    callback.onError("Error al procesar la respuesta del servidor.");
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callback.onError("Error de conexión: " + t.getMessage());
            }
        });
    }


    public void realizarTransferencia(String cuentaOrigen, String cuentaDestino, double importe, ConfigCallback<String> callback) {
        Call<ResponseBody> call = apiService.hacerTransferencia(cuentaOrigen, cuentaDestino, importe);
        call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                try {
                    if (response.isSuccessful()) {
                        String mensaje = response.body() != null ? response.body().string() : "Respuesta vacía del servidor";
                        callback.onSuccess(mensaje.trim());
                    } else {
                        String errorBody = response.errorBody() != null ? response.errorBody().string() : "Error desconocido";
                        callback.onError("Error del servidor: " + errorBody.trim());
                    }
                } catch (Exception e) {
                    callback.onError("Error al procesar la respuesta del servidor.");
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callback.onError("Error de conexión: " + t.getMessage());
            }
        });
    }


    public void realizarRetiro(String cuenta, double importe, ConfigCallback<String> callback) {
        Call<ResponseBody> call = apiService.hacerRetiro(cuenta, importe);
        call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                try {
                    if (response.isSuccessful()) {
                        String mensaje = response.body() != null ? response.body().string() : "Respuesta vacía del servidor";
                        callback.onSuccess(mensaje.trim());
                    } else {
                        String errorBody = response.errorBody() != null ? response.errorBody().string() : "Error desconocido";
                        callback.onError("Error del servidor: " + errorBody.trim());
                    }
                } catch (Exception e) {
                    callback.onError("Error al procesar la respuesta del servidor.");
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                callback.onError("Error de conexión: " + t.getMessage());
            }
        });
    }

}
