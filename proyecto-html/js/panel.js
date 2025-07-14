document.querySelectorAll('.dropdown-btn').forEach(btn => {
  btn.addEventListener('click', function() {
    this.classList.toggle('active');
    const dropdown = this.nextElementSibling;
    if (dropdown.style.display === "flex") {
      dropdown.style.display = "none";
    } else {
      dropdown.style.display = "flex";
    }
  });
});
