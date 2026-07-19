package ec.edu.ws_eurekabank_climov.modelo;

import com.google.gson.annotations.SerializedName;

public class DepositoResponse {
    @SerializedName("estado")
    private int estado;

    @SerializedName("mensaje")
    private String mensaje;

    @SerializedName("error")
    private String error;

    // Constructores
    public DepositoResponse() {}

    // Getters
    public int getEstado() { return estado; }
    public String getMensaje() { return mensaje; }
    public String getError() { return error; }
}
