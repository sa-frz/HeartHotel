let index = 0;
let programConductors;
let timerInterval;
let timeLeft;

function getProgramConductors(pc) {
    programConductors = pc;
    setTime();
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
    let currentTime = new Date().toLocaleTimeString('en-GB', { hour12: false });
    let time1 = programConductors.$values[index].SaatAz;
    let time2 = programConductors.$values[index].SaatTa;
    $('#Name').text(programConductors.$values[index].Name);
    $('#Description').text(programConductors.$values[index].Description);
    timeLeft = Math.abs(new Date(`1970-01-01T${time2}:00Z`) - new Date(`1970-01-01T${currentTime}Z`)) / 1000;
    if (index >= programConductors.$values.length) {
        index = 0;
        return;
    }
    index++;
    timerInterval = setInterval(updateTimer, 1000);
}