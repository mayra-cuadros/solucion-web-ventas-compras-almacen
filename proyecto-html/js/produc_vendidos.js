const ventas = [
  { nombre: "Laptop HP", precio: 2800.00, cantidad: 3, stock: 10, fecha: "2025-07-19", actualizado: "2025-07-19 11:00" },
  { nombre: "Teclado Logitech", precio: 120.00, cantidad: 6, stock: 14, fecha: "2025-07-19", actualizado: "2025-07-19 09:25" },
  { nombre: "Mouse Inalámbrico", precio: 75.00, cantidad: 9, stock: 22, fecha: "2025-07-18", actualizado: "2025-07-18 15:00" }
];

const fechaInput = document.getElementById("fecha");
const tabla = document.getElementById("tabla-productos");

fechaInput.addEventListener("change", () => {
  const fechaSeleccionada = fechaInput.value;
  const filtrados = ventas.filter(v => v.fecha === fechaSeleccionada);
  tabla.innerHTML = "";

  if (filtrados.length === 0) {
    tabla.innerHTML = '<tr><td colspan="5" class="text-center">No hay productos vendidos en esta fecha</td></tr>';
    return;
  }

  filtrados.forEach(p => {
    tabla.innerHTML += `
      <tr>
        <td>${p.nombre}</td>
        <td>S/. ${p.precio.toFixed(2)}</td>
        <td>${p.cantidad}</td>
        <td>${p.stock}</td>
        <td>${p.actualizado}</td>
      </tr>
    `;
  });
});

// Mostrar automáticamente ventas de hoy si hay
window.addEventListener("load", () => {
  const hoy = new Date().toISOString().split("T")[0];
  fechaInput.value = hoy;
  fechaInput.dispatchEvent(new Event("change"));
});
