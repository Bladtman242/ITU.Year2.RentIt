//TODO: Pop State

//Framework object, "namespace" for all framework functions.
var framework = {
    /**
     * Name of default page to be loaded if none is specified.
     */
    defaultPage: "about",
    
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
    loadPage: function(pageName,pushState,callback) {
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
            });
            
            //TODO: Push State
        }
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
