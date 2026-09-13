// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Admin sidebar toggle (mobile)
document.addEventListener('DOMContentLoaded', function () {
	var btn = document.getElementById('sidebarToggleBtn');
	var sidebar = document.getElementById('adminSidebar');
	if (btn && sidebar) {
		btn.addEventListener('click', function () {
			sidebar.classList.toggle('sidebar-open');
			var expanded = sidebar.classList.contains('sidebar-open') ? 'true' : 'false';
			btn.setAttribute('aria-expanded', expanded);
		});
	}
});
