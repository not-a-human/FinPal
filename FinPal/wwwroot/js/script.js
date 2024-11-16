function logToConsole(obj) {
    console.log(obj);
}

function setTheme(theme) {
    var mainPageBG = document.getElementById("mainPageBG");

    document.documentElement.setAttribute("data-bs-theme", theme);

    if (theme == "dark") {
        mainPageBG.classList.remove("light-mode-bg");
    } else {
        mainPageBG.classList.add("light-mode-bg");
    }
}

function scrollToTop() {
    console.log("scrollToTop  called");
    window.scrollToTop();
}

function pieChart(id, legend, myArray, salary = 0) {
    var ctx = document.getElementById(id);
    const legendContainer = document.getElementById(legend);

    // Create the table
    const table = document.createElement('table');
    table.classList.add('table', 'table-sm', 'align-self-center', 'mb-0');

    // Create the table body
    const tbody = document.createElement('tbody');

    var myPieChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: [],
            datasets: [{
                data: [],
                backgroundColor: [],
                borderColor: 'rgba(0, 0, 0, 0)',
                fill: true, // To show a filled area beneath the line
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            plugins: {
                legend: {
                    display: false
                }
            }, 
            cutoutPercentage: 80,
        },
    });

    myArray.forEach(element => {
        let randomColor = getRandomColor();
        const textNode = document.createTextNode(` ${element.name}`);

        myPieChart.data.datasets[0].data.push(element.value);
        myPieChart.data.datasets[0].backgroundColor.push(randomColor);

        // Create a table row
        const tr = document.createElement('tr');
        tr.classList.add('border-0');

        // Icon cell
        const iconCell = document.createElement('td');
        iconCell.classList.add('border-0');
        const icon = document.createElement('i');
        icon.classList.add('bi', 'bi-circle-fill'); // Bootstrap icon
        icon.style.color = randomColor;
        iconCell.appendChild(icon);
        tr.appendChild(iconCell);

        // Name cell
        const nameCell = document.createElement('td');
        nameCell.classList.add('text-left', 'border-0');
        nameCell.textContent = element.name;
        tr.appendChild(nameCell);

        // Value cell
        const valueCell = document.createElement('td');
        valueCell.classList.add('border-0');

        if (salary == 0) {
            valueCell.textContent = element.value;
        } else {
            valueCell.textContent = element.value + "%";
        }
        
        tr.appendChild(valueCell);

        // Append the row to the tbody
        tbody.appendChild(tr);

        if (salary != 0) {
            element.name += " - " + (salary * (element.value / 100)).toFixed(2) + " $";
        }

        myPieChart.data.labels.push(element.name);
    });

    // Append the tbody to the table
    table.appendChild(tbody);

    legendContainer.appendChild(table);

    myPieChart.update();
    console.log(myArray);
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

function hideOrShowElement(elementID) {
    var el = document.getElementById(elementID);
    el.classList.toggle("d-none");
}

function showToast(title, message) {
    // Get the container element
    const toastContainer = document.getElementById("mainToastContainer");

    // Create the toast element
    const toastElement = document.createElement("div");
    toastElement.className = "toast fade show";
    toastElement.setAttribute("role", "alert");
    toastElement.setAttribute("aria-live", "assertive");
    toastElement.setAttribute("aria-atomic", "true");

    // Create the header element
    const toastHeader = document.createElement("div");
    toastHeader.className = "toast-header";

    // Strong element for the title
    const strong = document.createElement("strong");
    strong.className = "me-auto";
    strong.textContent = title;

    // Small element for the time
    const small = document.createElement("small");
    small.className = "text-body-secondary";
    small.textContent = "";

    // Button element for close
    const button = document.createElement("button");
    button.className = "btn-close";
    button.setAttribute("type", "button");
    button.setAttribute("data-bs-dismiss", "toast");
    button.setAttribute("aria-label", "Close");

    // Append title, time, and button to the header
    toastHeader.appendChild(strong);
    toastHeader.appendChild(small);
    toastHeader.appendChild(button);

    // Create the body element
    const toastBody = document.createElement("div");
    toastBody.className = "toast-body";
    toastBody.textContent = message;

    // Append the header and body to the toast element
    toastElement.appendChild(toastHeader);
    toastElement.appendChild(toastBody);

    // Append the toast element to the toast container
    toastContainer.appendChild(toastElement);
}