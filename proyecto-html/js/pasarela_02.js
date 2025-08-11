function generarCodigoPedido() {
  return "PED-" + Math.floor(Math.random() * 900000 + 100000);
}

function cargarConfirmacion() {
  const metodo = localStorage.getItem("metodoPago") || "No registrado";
  const estado = localStorage.getItem("estadoPago") || "Desconocido";

  document.getElementById("codigo-pedido").textContent = generarCodigoPedido();
  document.getElementById("metodo-pago").textContent = metodo;
  document.getElementById("estado-pedido").textContent = estado;

  
  localStorage.removeItem("metodoPago");
  localStorage.removeItem("estadoPago");
}

function irATienda() {
  window.location.href = "Tienda.html";
}

document.addEventListener("DOMContentLoaded", cargarConfirmacion);
// pasarela_02.js