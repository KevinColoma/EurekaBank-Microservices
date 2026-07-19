package ec.edu.ws_eurekabank_climov.modelo;

import java.util.List;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface EurekabankService{
    @POST("validar-usuario")
    Call<ResponseBody> login(
            @Query("usuario") String usuario,
            @Query("password") String password
    );

    @POST("deposito")
    Call<ResponseBody> hacerDeposito(
            @Query("cuenta") String cuenta,
            @Query("importe") double importe
    );

    @POST("retiro")
    Call<ResponseBody> hacerRetiro(
            @Query("cuenta") String cuenta,
            @Query("importe") double importe
    );

    @POST("transferencia")
    Call<ResponseBody> hacerTransferencia(
            @Query("cuentaOrigen") String cuentaOrigen,
            @Query("cuentaDestino") String cuentaDestino,
            @Query("importe") double importe
    );

    @GET("movimientos/{cuenta}")
    Call<List<Movimiento>> obtenerMovimientos(@Path("cuenta") String cuenta);
}
