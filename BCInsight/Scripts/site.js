//Reset all validation message and filds 14-sep-2017 
$("input[type=reset]").click(function () {

    // get the form inside we are working - change selector to your form as needed
    var $form = $("form");

    // get validator object
    var $validator = $form.validate();

    // get errors that were created using jQuery.validate.unobtrusive
    var $errors = $form.find(".field-validation-error span");

    // trick unobtrusive to think the elements were succesfully validated
    // this removes the validation messages
    $errors.each(function () { $validator.settings.success($(this)); });

    // clear errors from validation
    $validator.resetForm();
});

//For Display Notification Messages
function Notyfication(type, message, layout) {
    var n = noty({
        text: message,
        type: type,
        dismissQueue: true,
        timeout: 10000,
        closeWith: ['click'],
        layout: layout,
        theme: 'defaultTheme',
        maxVisible: 10
    });
    console.log('html: ' + n.options.id);
}

function Site() {
    var self = this;
    self.Notification = function () {
        var $notificationBox = $('#NotificationBox');
        if ($notificationBox.length) {
            $("#NotificationBox").fadeIn(1000);
            $("#NotificationBox").delay(1000).fadeOut(1000);
        }
    }
}

var $site = new Site();
$(function () {
    callAllMethods($site);
});

function callAllMethods(obj) {
    // call every method of the given object
    for (var method in obj) {
        if (obj.hasOwnProperty(method) && typeof (obj[method]) === "function") {
            obj[method]();
        }
    }
}