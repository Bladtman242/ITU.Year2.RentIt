//Verifies that a user is currently logged in. If this is not the case, the 
//user is redirected to the default page.
if(!framework.user) {
    framework.loadPage(framework.defaultPage);
    
    var restrictedModal = $(
        "<div class='restrictModal' class='modal'> <div class='modal-header'>\
            <button type='button' class='close' data-dismiss='modal'>&times</button>\
            <div id='rentModalHeader'/><h3>Restricted access!</h3></div>\
            <div id='rentModalBody' class='modal-body'>\
                <p>You must be logged in to view the page you tried to access. Please log in or register.</p>\
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