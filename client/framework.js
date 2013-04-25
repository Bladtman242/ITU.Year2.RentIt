//Framework object, "namespace" for all framework functions.
var framework = {
    /**
     * Name of default page to be loaded if none is specified.
     */
    defaultPage: "about",
    
    /**
     * Title of site.
     */
    siteTitle: "moofy",
    
    /**
     * Main container of content. This should be a jQuery wrapped DOM element.
     * This is initialized on document ready.
     */
    container: null,
    
    /**
     * loadPage loads a page from its given name. The pushState arguments determines whether
     * a new history frame should be pushed (default true). This is used to preserve history.
     * If set to false, the step will not be recalled in history.
     */
    loadPage: function(pageName,pushState) {
            //Push State default true
            if(pushState == null) pushState = true;
            
            //Get page into #container
            framework.container.load(pageName+".htm", function() {
            
                //Link bindings (for internal links) - rebound on every page load as new links appear.
                $("a.internal").click(function() { //Live: constantly looking for changes in the DOM, binding matches' onClick.
                    var href = $(this).attr("href");
                    
                    //Validate link as internal
                    if(true) {
                        framework.loadPage(href.substring(1));
                        
                        //Stop link from working normally
                        return false;
                    }
                    
                    //Else alert error, and allow link to function normally.
                    alert("A link with the \"internal\" class with an invalid (external-pointing) href was clicked.");
                    return true;
                });
                
                //Push history frame.
                if(pushState) history.pushState({"page": pageName}, pageName, "#"+pageName);
            });
        }
};

//Handle history pop-event: Pop history frame (back/forward navigration)
window.onpopstate = function(event) {
    var page = framework.defaultPage;
    if(event.state != null) page = event.state.page;
    framework.loadPage(page,false);
};

$(document).ready(function() {
    framework.container = $('#container');
    
    //If a page (hash) is set:
    if(location.hash != "") {
        framework.loadPage(location.hash.substring(1),false);
    }
    else {
        framework.loadPage(framework.defaultPage,false);
    }
});
