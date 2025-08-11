// js/jstienda.js

// Productos simulados array simulado de productos (como si vinieran de una base de datos)
// cada producto tiene su propio id que puedes usar como "código del producto"
// js/jstienda.js

const productos = [
  {
    id: 2536,
    nombre: "Mochila Viajera Azul",
    precio: 120.00,
    imagen: "../img/mochilaviaje1.jpeg",
    categoria: "mochilaviaje"
  },
  {
    id: 2,
    nombre: "Bolsostich",
    precio: 89.50,
    imagen: "../img/Bolsostich.jpg",
    categoria: "bolsoniños"
  },
  {
    id: 3,
    nombre: "Bolso de Dama Rosa",
    precio: 149.90,
    imagen: "../img/bolsodama1.jpeg",
    categoria: "bolsos de dama"
  },
  {
    id: 4,
    nombre: "Mochila Viajera Negra",
    precio: 135.00,
    imagen: "../img/nochilaviajenegra.jpeg",
    categoria: "mochilaviaje"
  },
  {
    id: 5,
    nombre: "Bolso Capitán America",
    precio: 95.00,
    imagen: "../img/bolsocapitanamerica.jpeg",
    categoria: "bolsosniños"
  },
  {
    id: 6,
    nombre: "Mochila de Viaje Gris",
    precio: 110.00,
    imagen: "../img/mochilaviaje3.png",
    categoria: "mochilaviaje"
  },
  {
    id: 7,
    nombre: "Bolso de Dama",
    precio: 160.00,
    imagen: "../img/bolsodama3.jpeg",
    categoria: "bolsosdama"
  },
  {
    id: 8,
    nombre: "Bolso Astronauta",
    precio: 92.00,
    imagen: "../img/bolsoastronauta.jpg",
    categoria: "bolsosniños"
  }
];
// Inicializar el contador de carrito
//document.addEventListener("DOMContentLoaded", () => {
  //actualizarContador();
//});

// Mostrar productos según categoría
function mostrarProductos(filtro = "all") {
  const contenedor = document.getElementById("product-list");
  contenedor.innerHTML = "";

  const filtrados = filtro === "all" ? productos : productos.filter(p => p.categoria === filtro);

  if (filtrados.length === 0) {
    document.getElementById("no-products").classList.remove("hidden");
    return;
  }

  document.getElementById("no-products").classList.add("hidden");

  filtrados.forEach(prod => {
    const card = document.createElement("div");
    card.className = "product-card";
    card.innerHTML = `
      <img src="${prod.imagen}" alt="${prod.nombre}">
      <h3>${prod.nombre}</h3>
      <p><strong>Código:</strong> ${prod.id}</p>
      <p>S/ ${prod.precio.toFixed(2)}</p>
      <button onclick="agregarAlCarrito(${prod.id})">Agregar al carrito</button>
    `;
    contenedor.appendChild(card);
  });
}

// Agregar producto al carrito local
function agregarAlCarrito(idProducto) {
  const producto = productos.find(p => p.id === idProducto);
  let carrito = JSON.parse(localStorage.getItem("carrito")) || [];

  const existe = carrito.find(item => item.id === idProducto);
  if (existe) {
    existe.cantidad += 1;
  } else {
    carrito.push({ ...producto, cantidad: 1 });
  }

  localStorage.setItem("carrito", JSON.stringify(carrito));
  actualizarContador();
  mostrarToast(producto.nombre, calcularTotal(carrito));
}

// Contador
function actualizarContador() {
  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  const total = carrito.reduce((acc, item) => acc + item.cantidad, 0);
  document.getElementById("cart-count").innerText = total;
}

// Mostrar notificación
function mostrarToast(nombreProducto, total) {
  document.getElementById("toast-producto").innerText = `✔️ ${nombreProducto} agregado`;
  document.getElementById("toast-total").innerText = `Total actual: S/ ${total.toFixed(2)}`;
  document.getElementById("toast").classList.remove("hidden");

  // Ocultar automáticamente a los 4s
  setTimeout(() => {
    document.getElementById("toast").classList.add("hidden");
  }, 4000);
}

// Cerrar toast manual
document.getElementById("cerrar-toast").addEventListener("click", () => {
  document.getElementById("toast").classList.add("hidden");
});

// Calcular total
function calcularTotal(carrito) {
  return carrito.reduce((acc, item) => acc + item.precio * item.cantidad, 0);
}

// Filtrar productos
function filtrarProductos() {
  const filtro = document.getElementById("categoryFilter").value;
  mostrarProductos(filtro);
}

// Ir al carrito
function irACarro() {
  window.location.href = "carrito_de_compras.html";
}

// Abrir panel carrito
function abrirCarrito() {
  const panel = document.getElementById("carrito-panel");
  panel.classList.remove("hidden");
  cargarCarritoEnPanel();
}

// Cerrar panel
function cerrarCarrito() {
  document.getElementById("carrito-panel").classList.add("hidden");
}

// Cargar carrito en panel lateral
function cargarCarritoEnPanel() {
  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  const lista = document.getElementById("lista-carrito");
  lista.innerHTML = "";

  let total = 0;

  carrito.forEach(item => {
    const li = document.createElement("li");
    li.innerText = `${item.nombre} x${item.cantidad} - S/ ${(item.precio * item.cantidad).toFixed(2)}`;
    lista.appendChild(li);
    total += item.precio * item.cantidad;
  });

  document.getElementById("total-carrito").innerText = total.toFixed(2);
}

// Finalizar compra
//function finalizarCompra() {
//  alert("✅ Compra finalizada (demo)");
//  localStorage.removeItem("carrito");
//  actualizarContador();
//  cerrarCarrito();
//  mostrarProductos();
//}

document.addEventListener("DOMContentLoaded", () => {
  mostrarProductos();
  actualizarContador();
});


// Redirige si no tienes carrito.html:
//if (!localStorage.getItem('carrito')) {  no tocar código, están en evaluacion 
 // window.location.href = "carrito.html";
//} 



  // Redirige si tienes carrito.html:
  // window.location.href = "carrito.html";



/* const cartIcon = document.querySelector('.cart');
cartIcon.addEventListener('click', () => {   esto está comentado y no se borra
  alert(`Tienes ${cartCount} producto(s) en el carrito.`);
}); */