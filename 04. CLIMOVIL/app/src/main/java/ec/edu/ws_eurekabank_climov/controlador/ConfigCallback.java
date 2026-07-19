package ec.edu.ws_eurekabank_climov.controlador;

public interface ConfigCallback<T>{
    void onSuccess(T result);
    void onError(String error);
}
