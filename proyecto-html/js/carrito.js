function cargarCarrito() {
  const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  const tabla = document.getElementById("lista-carrito");
  const totalSpan = document.getElementById("total-compra");
  const vacio = document.getElementById("carrito-vacio");
  const contenido = document.getElementById("carrito-contenido");
  const btnConfirmar = document.getElementById("confirmar-pedido");

  tabla.innerHTML = "";
  let total = 0;

  if (carrito.length === 0) {
    vacio.classList.remove("hidden");
    contenido.classList.add("hidden");
    btnConfirmar.disabled = true;
    return;
  }

  vacio.classList.add("hidden");
  contenido.classList.remove("hidden");
  btnConfirmar.disabled = false;

  carrito.forEach((item) => {
    const subtotal = item.precio * item.cantidad;
    total += subtotal;

    const fila = document.createElement("tr");
    fila.innerHTML = `
      <td><img src="${item.imagen}" alt="${item.nombre}" /><span>${item.nombre}</span></td>
      <td>S/ ${item.precio.toFixed(2)}</td>
      <td>
        <input type="number" min="1" value="${item.cantidad}" onchange="cambiarCantidad(${item.id}, this.value)">
      </td>
      <td>S/ ${subtotal.toFixed(2)}</td>
      <td><button onclick="eliminarProducto(${item.id})">🗑️</button></td>
    `;
    tabla.appendChild(fila);
  });

  totalSpan.textContent = total.toFixed(2);
}

function cambiarCantidad(id, nuevaCantidad) {
  let carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  const producto = carrito.find(p => p.id === id);
  if (producto) {
    producto.cantidad = parseInt(nuevaCantidad);
    localStorage.setItem("carrito", JSON.stringify(carrito));
    cargarCarrito();
  }
}

function eliminarProducto(id) {
  let carrito = JSON.parse(localStorage.getItem("carrito")) || [];
  carrito = carrito.filter(p => p.id !== id);
  localStorage.setItem("carrito", JSON.stringify(carrito));
  cargarCarrito();
}

function irAPasarela() {
  window.location.href = "pasarelas.html";
}

document.addEventListener("DOMContentLoaded", cargarCarrito);

  // Aquí puedes redirigir a la pasarela de pago

 // document.getElementById('cart').style.display = 'none' NO TOCA Aún


