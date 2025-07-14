document.addEventListener("DOMContentLoaded", () => {
    const links = document.querySelectorAll('.nav-list a[data-page]');
    const mainContent = document.getElementById('mainContent');

    const loadPageContent = (page) => {
        switch (page) {
            case 'usuario':
                mainContent.innerHTML = '<h2>Registrar Usuario</h2><p>Formulario para registrar usuario.</p>';
                break;
            case 'listadoUsuarios':
                mainContent.innerHTML = '<h2>Listado de Usuarios</h2><p>Tabla con usuarios registrados.</p>';
                break;
            case 'proveedor':
                mainContent.innerHTML = '<h2>Registrar Proveedor</h2><p>Formulario para registrar proveedor.</p>';
                break;
            case 'listadoProveedores':
                mainContent.innerHTML = '<h2>Listado de Proveedores</h2><p>Proveedores registrados.</p>';
                break;
            case 'pedidos':
                mainContent.innerHTML = '<h2>Registro de Pedidos</h2><p>Formulario para pedidos.</p>';
                break;
            case 'roles':
                mainContent.innerHTML = '<h2>Gestión de Roles</h2><p>Roles y permisos.</p>';
                break;
            case 'guiaSalida':
                mainContent.innerHTML = '<h2>Guía de Salida</h2><p>Formulario para guía.</p>';
                break;
            default:
                mainContent.innerHTML = '<h2>Bienvenido</h2><p>Selecciona una opción del menú para comenzar.</p>';
        }
    };

    links.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            const page = e.target.getAttribute('data-page');
            if (page) loadPageContent(page);
        });
    });
});
