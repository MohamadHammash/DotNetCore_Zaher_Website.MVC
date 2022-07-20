//add a new style 'foo'
//$.notify.addStyle('foo', {
//    html:
//        "<div>" +
//        "<div class='clearfix'>" +
//        "<div class='title' data-notify-html='title'/>" +
//        "</div>" +
//        "</div>" +
//        "</div>"
//});

//$.notify.addStyle("foo", {
//    html: "<div class='modal' tabindex='-1'><div class='modal-dialog'><div class='modal-content'><div class='modal-header'><h5 class='modal-title'><button type='button' class='btn-close' data-bs-dismiss='modal' aria-label='Close'></button></div><div class='modal-body'><div class='alert' data-notify-html='title'></div></div></div></div></div>",
//    classes: {
//        base: {
//        }
//    }
//});

$.notify.addStyle("foo", {
    html: "<div class='wrap'><div class='notify-modal is-active js-notify-modal'><div class='notify-modal-image'><svg viewBox='0 0 32 32' style='fill:#48DB71'><path d='M1 14 L5 10 L13 18 L27 4 L31 8 L13 26 z'></path></svg></div><h1 class='modal-text' data-notify-html='title'></h1></div></div>",
    classes: {
        base: {
        }
    }
});

let successOptions2 = {
    style: "foo",
    autoHideDelay: 400,
    showAnimation: "fadeIn",
    hideAnimation: "fadeOut",
    hideDuration: 700,
    arrowShow: false,
};

const urlParams = new URLSearchParams(window.location.search);
const notification = urlParams.get('notify');

switch (notification) {
    case "Section-Edited":

        sendNotification('تم تعديل الباب');
        break;
    case "Section-Created":
        sendNotification('تمت إضافة باب جديد');
        break;
    case "QA-Edited":
        sendNotification('تم تعديل السؤال');
        break;
    case "QA-Created":
        sendNotification('تم إرسال سؤالك بنجاح');
        break;
    case "QA-Answered":
        sendNotification('تمت الإجابة على السؤال');
        break;
    case "FBook-Created":
        sendNotification('تمت إضافة قسم جديد');
        break;
    case "FBook-Edited":
        sendNotification('تم تعديل القسم');
        break;
    case "VList-Created":
        sendNotification('تمت إضافة قائمة فيديوهات جديدة');

        break;
    case "VList-Edited":
        sendNotification('تم تعديل القائمة');

        break;
    case "Video-Created":
        sendNotification('تمت إضافة مقطع للقائمة');

        break;
    case "Video-Edited":
        sendNotification('تم تعديل المقطع');

        break;
    case "PCard-Created":
        sendNotification('تمت إضافة بطاقة دعوية');
        break;
    case "PCard-Edited":
        sendNotification('تم تعديل البطاقة');

        break;
    case "FBook-Deleted":
        sendNotification('تم إزالة القسم');
        break;
    case "Section-Deleted":
        sendNotification('تم إزالة الباب');
        break;
    case "VList-Deleted":
        sendNotification('تم إزالة القائمة');
        break;
    case "Video-Deleted":
        sendNotification('تم إزالة المقطع');
        break;
    case "PCard-Deleted":
        sendNotification('تم إزالة البطاقة');
        break;
    case "QA-Deleted":
        sendNotification('تم إزالة السؤال');
        break;
    case "User-Created":
        sendNotification('تم إضافة المستخدم');
        break;
    case "User-Edited":
        sendNotification('تم تعديل المستخدم');
        break;
    case "User-Deleted":
        sendNotification('تم إزالة المستخدم');
        break;

    default:
}

function sendNotification(str) {
    $.notify({
        title: `${str}`,
    }, {
        style: 'foo',
        autoHide: true,
        clickToHide: true
    });
}

//if (notification == "QA-Created") {
//    $.notify({
//        title: 'تم إضافة سؤالك بنجاح',

//    }, {
//        style: 'foo',
//        autoHide: true,
//        clickToHide: true

//    });
//}