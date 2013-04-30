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

    var templateActionModal = $(
        "<div class='modal hide'>\
            <div class='modal-header'>\
                <button type='button' class='close' data-dismiss='modal'>&times</button>\
                <h3/>\
            </div>\
            <div class='modal-body'/>\
            <div class='modal-footer'>\
                <button class='btn' data-dismiss='modal'>Cancel</button>\
                <button class='btn btn-primary'/>\
            </div>\
        </div>");

    var templateModal = $(
        "<div class='modal hide'>\
            <div class='modal-header'>\
                <button type='button' class='close' data-dismiss='modal'>&times</button>\
                <h3/>\
            </div>\
            <div class='modal-body'/>\
            <div class='modal-footer'>\
                <button class='btn' data-dismiss='modal'>Cancel</button>\
            </div>\
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

    this.modal = function(header, body, actionButtonText, actionFunction) {
        $('.modal').remove();
        if (actionButtonText && actionFunction) { //use the action modal
            templateActionModal.children().eq(0).children().eq(1).text(header);
            templateActionModal.children().eq(1).html(body);
            var button = templateActionModal.children().eq(2).children().eq(1);
            button.text(actionButtonText);
            button.click(function() {
                templateActionModal.modal('hide'); //Hide the modal window
                actionFunction();
            });
            framework.container.prepend(templateActionModal);
            templateActionModal.modal(null);
        }else { //Use the one with the cancel button only
            templateModal.children().eq(0).children().eq(1).text(header);
            templateModal.children().eq(1).html(body);
            framework.container.prepend(templateModal);
            templateModal.modal(null);
        }
    };
};