package ec.edu.ws_eurekabank_climov.modelo;

import com.google.gson.annotations.SerializedName;

public class LoginResponse {
    @SerializedName("codigo")
    private String codigo;

    @SerializedName("usuario")
    private String usuario;

    @SerializedName("estado")
    private String estado;

    @SerializedName("error")
    private String error;

    // Constructores vacío
    public LoginResponse() {}

    // Getters y setters
    public String getCodigo() { return codigo; }
    public void setCodigo(String codigo) { this.codigo = codigo; }

    public String getUsuario() { return usuario; }
    public void setUsuario(String usuario) { this.usuario = usuario; }

    public String getEstado() { return estado; }
    public void setEstado(String estado) { this.estado = estado; }

    public String getError() { return error; }
    public void setError(String error) { this.error = error; }
}


