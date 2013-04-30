//TODO: When clicking links in the navbar, history sometimes breaks! (but only most of the time - sometimes it works).

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
     * Query string, from URI (between ? and optional #, eg. in ?movie=2#displayMovie, query string
     * will be "movie=2". This can be found in query._full. The value of a property can be found through
     * query.<property>; in this example query.movie will return 2.
     */
    query: {
        _full: location.search.substring(1)
    },
    
    /**
     * loadPage loads a page from its given name. The pushState arguments determines whether
     * a new history frame should be pushed (default true). This is used to preserve history.
     * If set to false, the step will not be recalled in history.
     */
    loadPage: function(pageName,pushState) {
            //Push State default true
            if(pushState == null) pushState = true;
            
            //Get page into #containe
            framework.container.load(pageName+".htm", function(response, status, xhr) {
            
                if(status == "error") {
                    framework.loadPage("404");
                    return;
                }
                
                //If this page is on the navigation bar - make sure it is shown as active
                $("#mainNav li a").each(function() {
                    if ($(this).attr('href') === '#' + pageName) {
                        $(this).parent().addClass('active');
                    } else {
                        $(this).parent().removeClass('active');
                    }
                });
            
                //Link bindings (for internal links) - rebound on every page load as new links appear.
                $("a.internal").unbind('click');
                $("a.internal").click(function() {
                    var href = $(this).attr("href");
                    
                    //Validate link as internal
                    if(true) {
                        framework.loadPage(href.substring(1));
                        
                        //Stop link from working normally
                        return false;
                    }
                    
                    //Else alert error, and allow link to function normally.
                    alert("A link with the \"internal\" class with an invalid (external-pointing) href was clicked.");
                    return false;
                });
                
                //Push history frame.
                if(pushState) history.pushState({"page": pageName}, pageName, "#"+pageName);
            });
        },
    
    /**
     * The currently logged in user, null if none is logged in.
     *
     * Contains id, username, email, name, isManager, balance
     *
     * NOTE: Currently temp. implementation until login page is implemented.
     * TODO: Pass around the user object in the state when loading a new page.
     */
    user: {
        id: 1,
        username: "SmallSon",
        name: "Captain Jack",
        email: "paulus@gmail.jp",
        isManager: true,
        balance: 1337
    }
};

//Load query properties into framework.query.
var qstr = framework.query._full;
var qstrLen = qstr.length;
var keyAccumulator = "";
var valueAccumulator = "";
for(var i = 0; i < qstrLen; i++) {
    if(qstr[i] == "=") {
        i++;
        for(; i < qstrLen; i++) {
            if(qstr[i] == "&") {
                framework.query[keyAccumulator] = valueAccumulator;
                keyAccumulator = "";
                valueAccumulator = "";
                break;
            }
            valueAccumulator += qstr[i];
        }
    }
    else {
        keyAccumulator += qstr[i];
    }
}
framework.query[keyAccumulator] = valueAccumulator;

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
        framework.loadPage(location.hash.substring(1));
    }
    else {
        framework.loadPage(framework.defaultPage);
    }
});
