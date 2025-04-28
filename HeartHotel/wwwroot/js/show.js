let index = 0;
let programConductors;
let timerInterval;
let timeLeft;

function getProgramConductors(pc) {
    programConductors = pc;
    setTime();

    $('#clock').html(new Date().toLocaleTimeString('en-GB', { hour12: false }));
    setInterval(() => {
        $('#clock').html(new Date().toLocaleTimeString('en-GB', { hour12: false }));
    }, 1000);
}


const timerElement = document.getElementById('time');
function updateTimer() {
    const hours = Math.floor(timeLeft / 3600);
    const minutes = Math.floor((timeLeft % 3600) / 60);
    const seconds = timeLeft % 60;
    timerElement.textContent = `${hours}:${minutes < 10 ? '0' : ''}${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    timeLeft--;

    if (timeLeft < 0) {
        clearInterval(timerInterval);
        //$.alert('زمان به پایان رسید!');
        setTime();
    }
}

function setTime() {
    $('#oldPrograms, #nextPrograms').html('');
    // let currentTime = new Date().toLocaleTimeString('en-GB', { hour12: false });
    const currentTime = new Date().toTimeString().slice(0, 5); // Get current time in HH:mm format
    for (var i = 0; (i < programConductors.$values.length); i++) {
        let time1 = programConductors.$values[i].SaatAz;
        let time2 = programConductors.$values[i].SaatTa;
        if (currentTime >= time1 && currentTime <= time2) {
            // now
            index = i;
        } else if (currentTime > time2) {
            $('#oldPrograms').append(`<tr><td>${programConductors.$values[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
        } else {
            $('#nextPrograms').append(`<tr><td>${programConductors.$values[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
        }
    }

    let SaatAz = programConductors.$values[index].SaatAz;
    let SaatTa = programConductors.$values[index].SaatTa;
    $('#Name').text(programConductors.$values[index].Name);
    $('#Description').text(programConductors.$values[index].Description);
    $('#times').html(`${SaatAz} - ${SaatTa}`);
    timeLeft = Math.abs(new Date(`1970-01-01T${SaatTa}:00Z`) - new Date(`1970-01-01T${currentTime}Z`)) / 1000 - 60;

    if (index >= programConductors.$values.length) {
        index = 0;
        return;
    }
    // index++;
    timerInterval = setInterval(updateTimer, 1000);
}

function adjustFontSizeToFit(elementId) {
    const element = document.getElementById(elementId);
    // element.textContent = text;
    let fontSize = 500; // Start with a base font size
    element.style.fontSize = fontSize + "px";

    while (element.scrollWidth > element.offsetWidth && fontSize > 10) {
        fontSize--;
        element.style.fontSize = fontSize + "px";
    }
}
