let index = 0;
let programConductors;
let timerInterval;
let timerIntervalSettime;
let timeLeft;
let hallNameOverlayTimer;
let hallNameTimer = 15000;
let clock;

function getProgramConductors(pc) {
    programConductors = pc;
    setTime();

    $('#clock').html(new Date().toLocaleTimeString('en-GB', { hour12: false }));
    clock = setInterval(() => {
        $('#clock').html(new Date().toLocaleTimeString('en-GB', { hour12: false }));
    }, 1000);
}

function SetShowHallName() {
    // if ($("#hallNameOverlay").hasClass('d-none')) {
    //     hallNameTimer = 3000; // Set timer to 3 seconds
    // } else {
    //     hallNameTimer = 15000; // Set timer to 15 seconds
    // }

    // Toggle visibility using fadeIn/fadeOut for smoother transitions
    $("#hallNameOverlay").toggleClass('d-none').toggleClass('d-flex');

    hallNameTimer = (hallNameTimer === 3000) ? 15000 : 3000; // Toggle between 3 and 15 seconds
    // clearInterval(hallNameOverlayTimer);
    hallNameOverlayTimer = setInterval(() => {
        SetShowHallName();
        clearInterval(hallNameOverlayTimer);
    }, hallNameTimer);
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
    let isSetIndex = false
    let oldPrograms = [], nextPrograms = [];
    // let currentTime = new Date().toLocaleTimeString('en-GB', { hour12: false });
    const currentTime = new Date().toTimeString().slice(0, 5); // Get current time in HH:mm format
    for (var i = 0; (i < programConductors.$values.length); i++) {
        let time1 = programConductors.$values[i].SaatAz;
        let time2 = programConductors.$values[i].SaatTa;
        if (currentTime >= time1 && currentTime <= time2) {
            // now
            index = i;
            isSetIndex = true;
        } else if (currentTime > time2) {
            oldPrograms.push(`<tr><td>${programConductors.$values[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
            // $('#oldPrograms').append(`<tr><td>${programConductors.$values[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
        } else {
            nextPrograms.push(`<tr><td>${programConductors.$values[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
            // $('#nextPrograms').append(`<tr><td>${programConductors.$values[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
        }
    }
    // console.log(oldPrograms, nextPrograms);


    if (!isSetIndex) {
        timerIntervalSettime = setInterval(setTime, 1000);
        $('#Name').closest('.gap-2').addClass('d-none');
        $('#oldPrograms').html(oldPrograms);
        $('#nextPrograms').html(nextPrograms);
    } else {
        clearInterval(timerIntervalSettime);
        $('#Name').closest('.gap-2').remove('d-none');
        let oldProgramsHtml = oldPrograms ? oldPrograms[oldPrograms.length - 1] : '';
        let nextProgramsHtml = nextPrograms ? nextPrograms.slice(0, 2).join('') : '';
        $('#oldPrograms').html(oldProgramsHtml);
        $('#nextPrograms').html(nextProgramsHtml);
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

function adjustFontSizeToFit(elementClass = 'adjustFontSizeToFit') {
    const $element = $('.' + elementClass);
    const isEnglish = /^[A-Za-z0-9\s.,'!?-]*$/.test($element.html());

    let fontSize = 300; // Start with a base font size
    $element.css('font-size', fontSize + 'px');
    $element.addClass('text-truncate');

    while ($element[0].scrollWidth > $element[0].offsetWidth && fontSize > 10) {
        fontSize--;
        $element.css('font-size', fontSize + 'px');
    }
}
