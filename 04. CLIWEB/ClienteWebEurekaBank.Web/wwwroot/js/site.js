document.addEventListener('submit', function (evento) {
    var formulario = evento.target;
    if (!formulario || formulario.getAttribute('data-activar-spinner') !== 'true') {
        return;
    }

    var boton = formulario.querySelector('[data-boton-spinner]');
    if (!boton) {
        return;
    }

    // Si jQuery Unobtrusive Validation está activo y el formulario no es válido, no mostramos spinner.
    if (typeof ($) !== 'undefined' && $(formulario).data('validator')) {
        if (!$(formulario).valid()) {
            return;
        }
    }

    var normal = boton.querySelector('.texto-normal');
    var cargando = boton.querySelector('.texto-cargando');

    if (normal) normal.classList.add('d-none');
    if (cargando) cargando.classList.remove('d-none');
    boton.setAttribute('disabled', 'disabled');
});
