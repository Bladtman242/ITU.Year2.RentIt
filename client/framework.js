//TODO: When clicking links in the navbar, history sometimes breaks! (but only most of the time - sometimes it works).

//Framework object, "namespace" for all framework functions.
var framework = {
    /**
     * Name of default page to be loaded if none is specified.
     */
    defaultPage: "home",
    
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
        _full: ""
    },
    
    loadQuery: function(q) {
        framework.query = {};
        framework.query._full = location.search.substring(1);
        
        if(q) framework.query._full = q;
        
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
    },
    
    /**
     * loadPage loads a page from its given name. The pushState arguments determines whether
     * a new history frame should be pushed (default true). This is used to preserve history.
     * If set to false, the step will not be recalled in history.
     */
    loadPage: function(pageName,pushState,query, callBack) {
            //Hide any modal windows on the current page
            $('.modal').modal('hide');
            //Push State default true
            if(pushState == null) pushState = true;
            //Query default empty
            if(query == null) {
                framework.loadQuery();
                query = framework.query._full;
            }
            else {
                framework.loadQuery(query);
            }
            
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
                
                framework.rebindInternalLinks();
                
                //Push history frame.
                if(pushState) history.pushState({"page": pageName, "query": query}, pageName, "?"+query+"#"+pageName);

                //Call potential callback
                if(callBack != null) {
                    callBack();
                }
            });
        },
    
    /**
     * Should be called after adding internal links dynamically.
     */
    rebindInternalLinks: function() {
            //Link bindings (for internal links) - rebound on every page load as new links appear.
                $("a.internal").unbind('click');
                $("a.internal").click(function() {
                    var href = $(this).attr("href");
                    
                    var linkValid = true;
                    
                    //Get query and hash from href and validate.
                    var hrefLen = href.length;
                    var query = "";
                    var hash = "";
                    for(var i = 0; i < hrefLen; i++) {
                        if(href[i] == "?") {
                            i++;
                            for(; i < hrefLen; i++) {
                                if(href[i] == "#") break;
                                query += href[i];
                            }
                        }
                        if(href[i] == "#") {
                            i++;
                            for(; i < hrefLen; i++) {
                                hash += href[i];
                            }
                        }
                    }
                    if(hash == "") linkValid = false;
                    
                    if(linkValid) {
                        framework.loadPage(hash,true,query);
                        
                        //Stop link from working normally
                        return false;
                    }
                    
                    //Else alert error, and allow link to function normally.
                    alert("A link with the \"internal\" class with an invalid href was clicked.");
                    return false;
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
    user: null
};

//Load query properties into framework.query.
framework.loadQuery();

//Handle history pop-event: Pop history frame (back/forward navigration)
window.onpopstate = function(event) {
    if(event.state != null) {
        var page = event.state.page;
        var query = event.state.query;
        framework.loadPage(page,false,query);
    }
};

$(document).ready(function() {
    framework.container = $('#container');
    
    //If a page (hash) is set:
    if(location.hash != "") {
        framework.loadPage(location.hash.substring(1),true,location.search.substring(1));
    }
    else {
        framework.loadPage(framework.defaultPage);
    }
});
