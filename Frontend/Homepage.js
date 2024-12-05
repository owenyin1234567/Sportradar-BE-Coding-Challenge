var Gamelistitem;

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

   
document.onreadystatechange = function (e) {
  if (document.readyState === 'complete') {
    Gamelistitem = document.querySelectorAll(".Game-list-item");
  }
}

async function GetAllGamesLoaded() {

  console.log('GetAllGamesLoaded')
  try {
    var options = {
      method: 'GET',
      url: 'https://localhost:7135/api/Game/GetAllGamesWithSport',
      headers: 
      {
        "Access-Control-Allow-Origin": "*"
      }
    }  
    var res = await axios.request(options);

    // Parse JSON data
    console.log('res', res.data)

    // Get the table body
    const tableBody = document.querySelector("tbody");
    tableBody.innerHTML = ""; // Clear existing rows

    const dayNames = [ "Monday", "Tuesday", "Wednesday" ,"Thursday", "Friday", "Saturday","Sunday"];

    // Populate table with data
    games = res.data;
    games.forEach(game => {
        const row = document.createElement("tr");
        const date = new Date(game.dateofevent);
        const day = date.getDay();
        const formattedDate = `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}, ${dayNames[day]}`;
        console.log(formattedDate)
        row.innerHTML = `
            <td>${game.dateofevent || "N/A"}</td>
            <td>${game.startingTime || "N/A"}</td>
            <td>${game.endTime || "N/A"}</td>
            <td>${game.homeTeam.name || "N/A"}</td>
            <td>${game.awayTeam.name || "N/A"}</td>
            <td>${game.sport_Game.name || "N/A"}</td>
        `;

        tableBody.appendChild(row)
  })}
  catch (err) {
    console.error(err);
  }
}