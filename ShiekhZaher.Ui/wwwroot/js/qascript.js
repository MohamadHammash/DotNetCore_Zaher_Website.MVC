////const { Doc } = require("../Assets/ckeditor_4.17.1_58999fff9e7f/ckeditor/samples/toolbarconfigurator/lib/codemirror/codemirror");

let qaContainerBtn = document.querySelector('#qa-accordion-container');
let buttons;
if (qaContainerBtn !== null) {

buttons = qaContainerBtn.getElementsByTagName('button');
}

function scrollUp(elementId) {
    window.scrollTo(0, 500);
}

function init() {
    for (let i = 0; i < buttons.length; i++) {
        const element = buttons[i];

        let attr = element.getAttribute('aria-controls');

        if (window.addEventListener) {
            buttons[i].addEventListener('click', function () {
                scrollUp(attr)
            });
        } else if (window.attachEvent) {
            buttons[i].attachEvent('click', function () {
                scrollUp(attr)
            });
        }
    }
}

if (window.addEventListener) {
    document.addEventListener("DOMContentLoaded", init, false);
} else if (window.attachEvent) {
    document.attachEvent("onDOMContentLoaded", init);
}



$('#qa-submit-form').submit(function () {
    let $this = $(this);
    if ($(this).find('.input-validation-error').length == 0) {

        $(this).find(':submit').val('يتم الإرسال .. الرجاء الانتظار');
        $(this).find(':submit').attr('disabled', 'disabled');

        setTimeout(function () {
            $this.find(':submit').val('أرسل السؤال');
            $this.find(':submit').removeAttr("disabled");
        }, 3000);
      
    }
   
});

//function timeoutEnabele() {
//    setTimeout(enableAgain, 5);
//}

//function enableAgain() {
//    $(this).find(':submit').val('أرسل السؤال');
//    $(this).find(':submit').removeAttr("disabled");
//}

//let qaSubmitForm = $("#qa-submit-form");

//qaSubmitForm.on("submit", function () {
//    $('#qa-submit-form').submit(function () {
//        if ($(this).find('.input-validation-error').length == 0) {

//            $(this).find(':submit').val('يتم الإرسال .. الرجاء الانتظار');
//            $(this).find(':submit').attr('disabled', 'disabled');
//        }
//        })

//}

//function stopDoubleSubmitionQa() {

//$('#qa-submit-form').submit(function () {
//    if ($(this).find('.input-validation-error').length == 0) {

//        $(this).find(':submit').val('يتم الإرسال .. الرجاء الانتظار');
//        $(this).find(':submit').attr('disabled', 'disabled');
//    }
//});
