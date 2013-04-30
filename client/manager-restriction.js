if(!framework.user.admin) {
    framework.loadPage(framework.defaultPage);
    
    var restrictedModal = $(
        "<div class='restrictModal' class='modal'> <div class='modal-header'>\
            <button type='button' class='close' data-dismiss='modal'>&times</button>\
            <div id='rentModalHeader'/><h3>Restricted access!</h3></div>\
            <div id='rentModalBody' class='modal-body'>\
                <p>Users must have manager access to view the manager overview! You are not a manager.</p>\
                <p>You have been redirected to the frontpage.</p>\
            </div>\
            <div class='modal-footer'>\
                <button class='restrictModalCancelButton' class='btn' data-dismiss='modal'>Okay - I regret snooping around.</button>\
            </div>\
        </div>");
        
    $("body").append(restrictedModal);
    $(".restrictModal .restrictModalCancelButton").click(function() {
        $(".restrictModal").remove();
    });
}