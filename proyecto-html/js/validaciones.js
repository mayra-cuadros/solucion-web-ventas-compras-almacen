function ValidarFormulario() {
    
  
  const producto = document.getElementById("ProductoID").value;
  const cantidad = document.getElementById("CantidadID").value;
  const destino = document.getElementById("DestinoID").value;
  const fecha = document.getElementById("FechaID").value;
  const nombre = document.getElementById("NombreID").value;

  if ( !producto || !cantidad || !destino || !fecha|| !nombre) {
    alert("Por favor, complete todos los campos.");
    return false;
  }

  else

  alert("Formulario enviado con éxito.");
  return true;
}