const links = document.querySelectorAll('.nav-link');

links.forEach(link => {
  link.addEventListener('click', function (e) {
    e.preventDefault(); // Evita navegación
    links.forEach(el => el.classList.remove('active')); // Quita a todos
    this.classList.add('active'); // Agrega al clicado
  });
});
