const instrucciones = document.getElementById("instrucciones");
const radios = document.querySelectorAll("input[name='pago']");
const btnConfirmar = document.getElementById("btn-confirmar");
let metodoSeleccionado = null;

radios.forEach(radio => {
  radio.addEventListener("change", () => {
    metodoSeleccionado = radio.value;
    btnConfirmar.disabled = false;
    mostrarInstrucciones(metodoSeleccionado);
  });
});

function mostrarInstrucciones(metodo) {
  switch (metodo) {
    case "efectivo":
      instrucciones.innerHTML = `
        <h3>💵 Pago en efectivo</h3>
        <p>Tu pedido será pagado al momento de la entrega. Ten el monto exacto listo.</p>
      `;
      break;
    case "tarjeta":
      instrucciones.innerHTML = `
        <h3>💳 Tarjeta de crédito o débito</h3>
        <form id="form-tarjeta">
          <label>Número de tarjeta:<br><input type="text" maxlength="16" required></label><br><br>
          <label>Nombre en la tarjeta:<br><input type="text" required></label><br><br>
          <label>Fecha de vencimiento:<br><input type="month" required></label><br><br>
          <label>CVC:<br><input type="text" maxlength="4" required></label>
        </form>
      `;
      break;
    case "yape":
      instrucciones.innerHTML = `
        <h3>📱 Yape</h3>
        <p>Escanea el siguiente código QR desde tu app Yape y realiza el pago.</p>
        <img src="../img/yape-qr.jpg" alt="QR Yape" width="150">
        <p>Luego confirma con el botón inferior.</p>
      `;
      break;
    case "plin":
      instrucciones.innerHTML = `
        <h3>📲 Plin</h3>
        <p>Escanea el código QR desde Plin y paga el total indicado.</p>
        <img src="../img/plin-qr.jpg" alt="QR Plin" width="150">
        <p>Confirma el pago con el botón inferior.</p>
      `;
      break;
    case "pagoefectivo":
      instrucciones.innerHTML = `
        <h3>🏦 PagoEfectivo</h3>
        <p>Tu código CIP es: <strong>951-246-987</strong></p>
        <p>Puedes pagar en agentes autorizados o banca por internet.</p>
      `;
      break;
  }
}

function confirmarPago() {
  if (!metodoSeleccionado) {
    alert("Por favor selecciona un método de pago válido.");
    return;
  }

  const esAutomatico = metodoSeleccionado === "tarjeta" || metodoSeleccionado === "pagoefectivo";

  
  if (esAutomatico) {
    alert("✅ Pago procesado exitosamente.");
    localStorage.setItem("estadoPago", "Confirmado");
  } else {
    alert("🕐 Pedido registrado. Pendiente de verificación.");
    localStorage.setItem("estadoPago", "Pendiente de verificación");
  }

  localStorage.setItem("metodoPago", metodoSeleccionado);
  localStorage.removeItem("carrito");

  
  window.location.href = "pasarela_02.html";
}





  // Aquí podrías enviar los datos al backend o almacenarlos
  mensaje.innerHTML = `✅ Pedido registrado con método: <strong>${metodo.value}</strong><br>Estado: <strong>${estado}</strong>`;

