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

function pieChart(id, legend, myArray) {
    var ctx = document.getElementById(id);
    const legendContainer = document.getElementById(legend);

    var myPieChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: [],
            datasets: [{
                data: [],
                backgroundColor: [],
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
                    display: false // Ensure this is set to false
                }
            }, 
            cutoutPercentage: 80,
        },
    });

    myArray.forEach(element => {
        let randomColor  = getRandomColor();
        myPieChart.data.labels.push(element.name);
        myPieChart.data.datasets[0].data.push(element.value);
        myPieChart.data.datasets[0].backgroundColor.push(randomColor);

        const span = document.createElement('span');
        span.classList.add('mr-2');

        const icon = document.createElement('i');
        icon.classList.add('bi', 'bi-circle-fill');
        icon.style.color = randomColor;

        const textNode = document.createTextNode(` ${element.name}`);

        span.appendChild(icon);
        span.appendChild(textNode);

        legendContainer.appendChild(span);
    });

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