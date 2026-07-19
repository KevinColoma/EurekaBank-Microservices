# 🎨 Identidad Visual - EurekaBank

## Colores Principales

- **Verde Marca**: `#95bf5a` - Usado para acentos, botones positivos y destacados
- **Azul Marca**: `#2661a9` - Color principal de navegación y encabezados
- **Blanco**: `#ffffff` - Fondos limpios y textos sobre colores oscuros
- **Negro**: `#000000` - Textos principales

## Diseño de Variables CSS

Se han definido variables CSS globales en los archivos de estilo:

```css
:root {
  --color-principal-verde: #95bf5a;
  --color-principal-azul: #2661a9;
  --color-blanco: #ffffff;
  --color-negro: #000000;
}
```

## Componentes Diseñados

### 🔐 Página de Login (`Vistas/Acceso/IniciarSesion.cshtml`)
- **Fondo**: Imagen `monsterFont.jpg` con overlay oscuro semi-transparente
- **Tarjeta**: Blanca con sombra y bordes redondeados
- **Entrada**: Título con color azul marca
- **Botones**: Gradiente azul para iniciar sesión
- **Campos**: Bordes personalizados en azul marca (2px)

### 📊 Panel Principal (`Vistas/Panel/Index.cshtml`)
- **Encabezado**: Gradiente de colores con imagen `Monsterworking.jpg`
- **Títulos**: Color azul marca
- **Usuario**: Nombre destacado en verde marca
- **Botones**: Combinaciones de verde y azul con gradientes
- **Tarjetas**: Bordes izquierdos de 4px en colores de marca

### 📝 Movimientos (`Vistas/Movimientos/Index.cshtml`)
- **Encabezado de tabla**: Fondo azul claro, texto azul marca, borde inferior
- **Badges**: Verde para ENTRADA, Rojo para SALIDA
- **Importes**: Coloreados según tipo de movimiento (verde/rojo)

### 💳 Operaciones (`Vistas/Operaciones/*`)

#### Depósito (`Deposito.cshtml`)
- Borde izquierdo verde marca (4px)
- Botón registrar en verde marca
- Campos con borde azul marca

#### Retiro (`Retiro.cshtml`)
- Borde izquierdo azul marca (4px)
- Botón registrar en rojo (operación de salida)
- Campos con borde azul marca

#### Transferencia (`Transferencia.cshtml`)
- Borde izquierdo verde marca (4px)
- Botón con gradiente verde marca
- Campos con borde azul marca

### 🧭 Navegación (`Views/Shared/_Layout.cshtml`)
- **Navbar**: Gradiente azul marca con logo personalizado
- **Logo**: Letra "E" en verde marca
- **Avatar**: `Ficha.jpg` redondeado (32x32px) en la barra superior
- **Footer**: Gradiente suave gris

## Imágenes Utilizadas

Las imágenes están ubicadas en `/wwwroot/images/`:

| Archivo | Uso | Dimensiones |
|---------|-----|-------------|
| `monsterFont.jpg` | Fondo de login | Fondo completo con overlay |
| `Monsterworking.jpg` | Dashboard decorativo | Máximo 150px |
| `Ficha.jpg` | Avatar de perfil | 32x32px |

## Responsive Design

Todos los elementos están optimizados para:
- 📱 Mobile (xs, sm)
- 💻 Tablet (md, lg)
- 🖥️ Desktop (xl, xxl)

## Accesibilidad

- ✅ Contraste de colores WCAG AA
- ✅ Etiquetas semánticas HTML
- ✅ Validación de formularios
- ✅ Mensajes de error claros

## Extensión Futura

Para mantener la identidad visual:

1. Use las variables CSS `--color-principal-verde` y `--color-principal-azul`
2. Aplique bordes izquierdos de 4px en tarjetas importantes
3. Use gradientes con los colores principales
4. Mantenga el espaciado y padding consistentes

---

**Última actualización**: 2026-05-15
**Versión**: 1.0
