package ec.edu.ws_eurekabank_climov.modelo;

public class Config {
    private Config(){}
    public static final String PUERTO = "5069";
    public static final String RUTA = "/Eureka/";
    public static String BASE_URL = "http://10.9.8.137:" + PUERTO + RUTA;

    public static void actualizarIp(String ip) {
        BASE_URL = "http://" + ip + ":" + PUERTO + RUTA;
    }
}
