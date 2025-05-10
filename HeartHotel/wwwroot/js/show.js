let index = 0;
let programConductors, allDaySessions, chairs, moderators, currentSession;
let timerInterval;
let timerIntervalSettime;
let timeLeft;
let timeAutoPrograms;
let hallNameOverlayTimer;
let hallNameTimer = 15000;
let clock;

async function getProgramConductors(auto, venueHallId, date, id) {
    // Show Hall Name Overlay
    $('#hallNameOverlay').removeClass('d-none').addClass('d-flex');
    adjustFontSizeToFit();
    setClock();

    if (auto) {
        try {
            const response = await $.get(`/api/program/AllDaySessions?VenueHallId=${venueHallId}&Date=${date}`);
            allDaySessions = response;
            showDataAuto();
            return;
        } catch (error) {
            console.error('Failed to fetch session data:', error);
            return;
        }
    } else {
        try {
            const pc = await $.get(`/api/program/Session?Id=${id}`);
            currentSession = pc;
            showDataOnce();
            return;
        } catch (error) {
            console.error('Failed to fetch session data:', error);
            return;
        }
    }
}

function showDataAuto() {
    // Show Hall Name Overlay
    $('#hallNameOverlay').removeClass('d-none').addClass('d-flex');
    adjustFontSizeToFit();

    let currentTime = new Date().toTimeString().slice(0, 5); // Get current time in HH:mm format
    let filteredSessions = allDaySessions.filter(session => {
        return currentTime >= session.minSaatAz && currentTime <= session.maxSaatTa;
    });
    // console.log('showDataAuto', currentTime);
    if (filteredSessions.length > 0) {
        clearTimeout(timeAutoPrograms);
        programConductors = filteredSessions[0].programConductors;
        chairs = filteredSessions[0].chairsConductors.filter(chair => {
            return chair.roleId == 1;
        });
        moderators = filteredSessions[0].chairsConductors.filter(moderator => {
            return moderator.roleId == 2;
        });

        let chairsHtml = chairs.map((chair, idx) => {
            return idx < chairs.length - 1 ? `<div>${chair.name},</div>` : `<div>${chair.name}</div>`;
        }).join('');
        $('#Chairs').html(`<div class="text-secondary fw-bold">Chairs: </div> ${chairsHtml}`);

        let moderatorsHtml = moderators.map((moderator, idx) => {
            return idx < moderators.length - 1 ? `<div>${moderator.name},</div>` : `<div>${moderator.name}</div>`;
        }).join('');
        $('#Moderators').html(`<div class="text-secondary fw-bold">Moderators: </div> ${moderatorsHtml}`);
        
        $('#title').html(filteredSessions[0].name);
    
        $('#hallNameOverlay').removeClass('d-flex').addClass('d-none');
        setTime(true);
    } else {
        console.warn('No sessions found for the current time.');
        clearTimeout(timeAutoPrograms);
        timeAutoPrograms = setTimeout(() => {
            showDataAuto();
        }, 1000);
        // return;
    }
}

function showDataOnce() {
    programConductors = currentSession[0].programConductors;
    chairs = currentSession[0].chairsConductors.filter(chair => {
        return chair.roleId == 1;
    });
    moderators = currentSession[0].chairsConductors.filter(moderator => {
        return moderator.roleId == 2;
    });

    let chairsHtml = chairs.map((chair, idx) => {
        return idx < chairs.length - 1 ? `<div>${chair.name},</div>` : `<div>${chair.name}</div>`;
    }).join('');
    $('#Chairs').html(chairsHtml);

    let moderatorsHtml = moderators.map((moderator, idx) => {
        return idx < moderators.length - 1 ? `<div>${moderator.name},</div>` : `<div>${moderator.name}</div>`;
    }).join('');
    $('#Moderators').html(moderatorsHtml);
    $('#title').html(currentSession[0].name);

    $('#hallNameOverlay').removeClass('d-flex').addClass('d-none');
    setTime();
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
function updateTimer(auto = false) {
    const hours = Math.floor(timeLeft / 3600);
    const minutes = Math.floor((timeLeft % 3600) / 60);
    const seconds = timeLeft % 60;
    timerElement.textContent = `${hours}:${minutes < 10 ? '0' : ''}${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    timeLeft--;

    if (timeLeft < 0) {
        clearInterval(timerInterval);
        //$.alert('زمان به پایان رسید!');
        setTime(auto);
    }
}

function setTime(auto = false) {
    $('#oldPrograms, #nextPrograms').html('');
    let isSetIndex = false
    let oldPrograms = [], nextPrograms = [];
    // let currentTime = new Date().toLocaleTimeString('en-GB', { hour12: false });
    const currentTime = new Date().toTimeString().slice(0, 5); // Get current time in HH:mm format
    for (var i = 0; (i < programConductors.length); i++) {
        let time1 = programConductors[i].saatAz;
        let time2 = programConductors[i].saatTa;
        if (currentTime >= time1 && currentTime < time2) {
            // now
            index = i;
            isSetIndex = true;
        } else if (currentTime >= time2) {
            oldPrograms.push(`<tr><td>${programConductors[i].name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
            // $('#oldPrograms').append(`<tr><td>${programConductors[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
        } else {
            nextPrograms.push(`<tr><td>${programConductors[i].name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
            // $('#nextPrograms').append(`<tr><td>${programConductors[i].Name}</td><td class="text-start">${time1} - ${time2}</td></tr>`);
        }
    }
    // console.log(oldPrograms, nextPrograms);


    if (!isSetIndex) {
        // Session is End
        clearTimeout(timerIntervalSettime);
        timerIntervalSettime = setTimeout(setTime, 1000);
        $('#Name').closest('.gap-2').addClass('d-none');
        $('#oldPrograms').html(oldPrograms);
        $('#nextPrograms').html(nextPrograms);
        if (auto) {
            clearTimeout(timerIntervalSettime);
            showDataAuto();
            return;
        }
    } else {
        clearTimeout(timerIntervalSettime);
        $('#Name').closest('.gap-2').remove('d-none');
        let oldProgramsHtml = oldPrograms ? oldPrograms[oldPrograms.length - 1] : '';
        let nextProgramsHtml = nextPrograms ? nextPrograms.slice(0, 2).join('') : '';
        $('#oldPrograms').html(oldProgramsHtml);
        $('#nextPrograms').html(nextProgramsHtml);
    }

    let SaatAz = programConductors[index].saatAz;
    let SaatTa = programConductors[index].saatTa;
    $('#Name').text(programConductors[index].name);
    $('#Description').text(programConductors[index].description);
    $('#times').html(`${SaatAz} - ${SaatTa}`);
    timeLeft = Math.abs(new Date(`1970-01-01T${SaatTa}:00Z`) - new Date(`1970-01-01T${currentTime}Z`)) / 1000 - 60;

    if (index >= programConductors.length) {
        index = 0;
        return;
    }
    // index++;
    clearInterval(timerInterval); // Clear any existing interval
    timerInterval = setInterval(() => {
        updateTimer(auto);
    }, 1000);
}

function adjustFontSizeToFit(elementClass = 'adjustFontSizeToFit') {
    const $element = $('.' + elementClass);
    const isEnglish = /^[A-Za-z0-9\s.,'!?-]*$/.test($element.html());

    let fontSize = 500; // Start with a base font size
    $element.css('font-size', fontSize + 'px');
    $element.addClass('text-truncate');

    while ($element[0].scrollWidth > $element[0].offsetWidth && fontSize > 10) {
        fontSize--;
        $element.css('font-size', fontSize + 'px');
    }

    fontSize -= 10; // Reduce font size a bit more for better fit
    $element.css('font-size', fontSize + 'px');

    while ($element[0].scrollWidth > $element[0].offsetWidth && fontSize > 10) {
        fontSize--;
        $element.css('font-size', fontSize + 'px');
    }
}

function setClock(show = true) {
    if (show) {
        $('#clock').html(new Date().toLocaleTimeString('en-GB', { hour12: false }));
        clock = setInterval(() => {
            $('#clock').html(new Date().toLocaleTimeString('en-GB', { hour12: false }));
        }, 1000);
    } else {
        $('#clock').html('');
        clearInterval(clock);
    }
}
