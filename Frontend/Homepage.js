function sortTable(columnIndex) {
  const table = document.getElementById("Game-list");
  const tbody = table.querySelector("tbody");
  const rows = Array.from(tbody.querySelectorAll("tr"));

  // Determine sorting order: ascending or descending
  const isAscending = table.dataset.sortOrder === "asc";
  table.dataset.sortOrder = isAscending ? "desc" : "asc";

  rows.sort((a, b) => {
      const aText = a.cells[columnIndex].innerText.trim();
      const bText = b.cells[columnIndex].innerText.trim();

      // Handle numeric sorting
      if (!isNaN(aText) && !isNaN(bText)) {
          return isAscending ? aText - bText : bText - aText;
      }

      // Handle date sorting
      const aDate = Date.parse(aText);
      const bDate = Date.parse(bText);
      if (!isNaN(aDate) && !isNaN(bDate)) {
          return isAscending ? aDate - bDate : bDate - aDate;
      }

      // Default to string sorting (case-insensitive)
      return isAscending
          ? aText.localeCompare(bText)
          : bText.localeCompare(aText);
  });

  // Append sorted rows back to the tbody
  rows.forEach((row) => tbody.appendChild(row));
}

   
