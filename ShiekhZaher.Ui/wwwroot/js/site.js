function getDate() {
    let date = new Intl.DateTimeFormat('ar-TN-u-ca-islamic', {
        day: 'numeric',
        month: 'long',
        weekday: 'long',
        year: 'numeric',
        era: 'narrow',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    }).format(Date.now());

    let str = date.toString().replace("في", ",");
    //console.log(str);

    let footerdate = document.querySelector('#footer-date');
    footerdate.innerHTML = str;
}

setInterval(getDate, 1000);
if (window.addEventListener) {
    document.addEventListener("DOMContentLoaded", getDate, false);
} else if (window.attachEvent) {
    document.attachEvent("onDOMContentLoaded", getDate);
}

/*upload buttons*/


document.querySelector(".upload").onchange = function () {
    let uploadValue = this.value;
    let last = uploadValue.lastIndexOf('\\');
    let subValue = uploadValue.substring(last + 1);

    document.querySelector(".uploadFile-disabled").value = subValue;
};

/*upload buttons end*/
//const { get } = require("jquery");