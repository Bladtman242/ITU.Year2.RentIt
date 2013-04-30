//Framework object, "namespace" for all framework functions.


var output = new function(){
    //Private variables
    var templateErrorAlert = $(
        "<div class='alert alert-error'>\
            <button type='button' class='close' data-dismiss='alert'>&times;</button>\
            <strong id='errorBold'/> <span id='errorText'/>\
        </div>");

    var templateSuccessAlert = $(
        "<div class='alert alert-success'>\
            <button type='button' class='close' data-dismiss='alert'>&times;</button>\
            <strong/> <span/>\
        </div>");

    //Public functions
    this.error = function(bold, nonBold) {
        templateErrorAlert.children().eq(1).html(bold);
        templateErrorAlert.children().eq(2).html(nonBold);
        $('.alert').remove();
        framework.container.prepend(templateErrorAlert);
    };

    this.success = function(bold, nonBold) {
        templateSuccessAlert.children().eq(1).html(bold);
        templateSuccessAlert.children().eq(2).html(nonBold);
        $('.alert').remove();
        framework.container.prepend(templateSuccessAlert);
    };

    this.modal = function(header, body, actionCallback) {
        $('.modal').remove

    };
};