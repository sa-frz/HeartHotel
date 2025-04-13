
String.prototype.toPesianNumbers = function () {
    var pn = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
    var en = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    var an = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
    var cache = this;
    for (var i = 0; i < 10; i++) {
        var regex_en = new RegExp(en[i], 'g');
        var regex_ar = new RegExp(an[i], 'g');
        cache = cache.replace(regex_en, pn[i]);
        cache = cache.replace(regex_ar, pn[i]);
    }

    return cache;
};

String.prototype.toEnglishNumbers = function () {
    var pn = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
    var en = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    var an = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
    var cache = this;
    for (var i = 0; i < 10; i++) {
        var regex_fa = new RegExp(pn[i], 'g');
        var regex_ar = new RegExp(an[i], 'g');
        cache = cache.replace(regex_fa, en[i]);
        cache = cache.replace(regex_ar, en[i]);
    }

    return cache;
};

const persianMobileRegex = /^(0|\+98)?9[0-9]{9}$/;

async function sendSMS(mobile) {
    const response = await fetch(`/api/sms/send?mobile=${mobile}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ mobile: mobile })
    });

    if (response.ok) {
        return true;
    } else {
        return false;
    }
}

async function checkCode(mobile, code) {
    const response = await fetch(`/api/sms/code?mobile=${mobile}&code=${code}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ mobile: mobile, code: code })
    });

    if (response.ok) {
        return true;
    } else {
        return false;
    }
}

async function lectureCreate(eventsPersonId, timesId, az, ta, txt) {
    const response = await fetch(`/api/event/presenter/lecture/create?eventsPersonId=${eventsPersonId}&timesId=${timesId}&saatAz=${az}&saatTa=${ta}&text=${txt}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            eventsPersonId: eventsPersonId,
            timesId: timesId,
            saatAz: az,
            saatTa: ta,
            text: txt
        })
    });

    if (response.ok) {
        return true;
    } else {
        return false;
    }
}